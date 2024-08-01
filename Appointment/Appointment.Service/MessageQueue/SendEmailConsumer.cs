using HealthMed.CrossCutting.QueueMessenge;
using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Text;

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

        StringBuilder sb = new();
        sb.AppendLine("Health&Med - Nova consulta agendada");
        sb.AppendLine($"Corpo do e-mail: Olá, Dr. {context.Message.DoctorName}");
        sb.AppendLine("Você tem uma nova consulta marcada!");
        sb.AppendLine($"Paciente: {context.Message.PatientName}.");
        sb.AppendLine($"Data e horário: {context.Message.Date:dd/MM/yyyy} às {context.Message.Time}..");

        message.Body = new TextPart("plain")
        {
            Text = sb.ToString()
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
