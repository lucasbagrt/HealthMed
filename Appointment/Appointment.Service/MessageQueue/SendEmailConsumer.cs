using HealthMed.CrossCutting.QueueMessenge;
using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Appointment.Service.QueueMessege;

public class SendEmailConsumer : IConsumer<CreateAppointment>
{
    public async Task Consume(ConsumeContext<CreateAppointment> context)
    {
        IConfiguration _configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true).Build();

        MimeMessage message = new();
        message.From.Add(new MailboxAddress("Health Med", "fiaphealthmed@gmail.com"));
        message.To.Add(new MailboxAddress(context.Message.DoctorName, context.Message.DoctorEmail));
        message.Subject = $"Consulta Confirmada!";

        string htmlBody = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 5px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                }}
                h1 {{
                    color: #333333;
                }}
                p {{
                    color: #666666;
                    line-height: 1.5;
                }}
                .highlight {{
                    color: #ff6600;
                    font-weight: bold;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <h1>Health&Med - Nova consulta agendada</h1>
                <p>Olá, Dr. {context.Message.DoctorName},</p>
                <p>Você tem uma nova consulta marcada!</p>
                <p>Paciente: <span class=""highlight"">{context.Message.PatientName}</span>.</p>
                <p>Data e horário: <span class=""highlight"">{context.Message.Date:dd/MM/yyyy}</span> às <span class=""highlight"">{context.Message.Time}</span>.</p>
            </div>
        </body>
        </html>";

        message.Body = new TextPart("html")
        {
            Text = htmlBody
        };

        using SmtpClient client = new();

        string smtpServer = _configuration["Smtp:Server"];
        int smtpPort = int.Parse(_configuration["Smtp:Port"]);
        string smtpUser = _configuration["Smtp:User"];
        string smtpPass = _configuration["Smtp:Pass"];

        await client.ConnectAsync(smtpServer, smtpPort, false);
        await client.AuthenticateAsync(smtpUser, smtpPass);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
