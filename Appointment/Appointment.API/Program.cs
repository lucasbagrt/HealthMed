using AutoMapper;
using HealthMed.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Service.Services;
using Appointment.API.Filters;
using Appointment.Infra.Data.Context;
using Appointment.Api.Mapper;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Infra.Data.Repositories;
using Appointment.Data.Repositories;
using Appointment.Service.Services;
using Appointment.Domain.Interfaces.Services;
using MassTransit;
using HealthMed.CrossCutting.QueueMessenge;
using Appointment.Domain.Interfaces.Integration;
using Appointment.Service.Integration;
using Appointment.Service.QueueMessege;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

#region [DB]
services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("local");
    options.UseSqlServer(connectionString);
});
#endregion

#region [Authentication]
services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });
#endregion

#region [Mapper]            
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
services.AddSingleton(mapper);
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region [DI]
services.AddScoped<NotificationContext>();
services.AddScoped<IBaseService, BaseService>(); 
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IAvailabilityService, AvailabilityService>();
services.AddScoped<IUserIntegration, UserIntegration>();

services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
#endregion

#region [Swagger]            
services.AddSwaggerGen();
services.AddCors();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Appointment v1", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Auth.
                                    Ex: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
});
#endregion

services.AddControllers(options =>
{
    options.Filters.Add<NotificationFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

#region [Service Bus]
var connection = builder.Configuration.GetSection("MassTransitAzure")["Connection"];
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AppointmentConsumer>();

    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(connection);

        cfg.Message<CreateAppointment>(x =>
        {
            x.SetEntityName("topico");
        });

        cfg.SubscriptionEndpoint("sub-1", "topico", e =>
        {
            e.ConfigureConsumer<AppointmentConsumer>(context);
        });

        cfg.SubscriptionEndpoint("sub-2", "topico", e =>
        {
            e.Consumer<SendEmailConsumer>();
        });
    });
});


services.AddMassTransitHostedService();
#endregion

var app = builder.Build();

#region [Migrations and Seeds]
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
    dbContext.EnsureSeedData(scope.ServiceProvider);
}
#endregion

#region [Swagger App]            
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment v1");
    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
});
#endregion

#region [Cors]            
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
#endregion

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();