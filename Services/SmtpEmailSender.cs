using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class SmtpEmailSender : IEmailSender
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _fromEmail;
    private readonly string _password;

    public SmtpEmailSender(string smtpServer, int port, string fromEmail, string password)
    {
        _smtpServer = smtpServer;
        _port = port;
        _fromEmail = fromEmail;
        _password = password;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            using var client = new SmtpClient(_smtpServer, _port)
            {
                Credentials = new NetworkCredential(_fromEmail, _password),
                EnableSsl = true
            };

            var fromAddress = new MailAddress(_fromEmail, "Agri Energy Connect"); // Could be from config
            var toAddress = new MailAddress(email);

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            // Add plain text alternative (strip tags or provide simple text)
            var plainText = System.Text.RegularExpressions.Regex.Replace(htmlMessage, "<.*?>", string.Empty);
            var plainView = AlternateView.CreateAlternateViewFromString(plainText, null, "text/plain");
            var htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage, null, "text/html");

            message.AlternateViews.Add(plainView);
            message.AlternateViews.Add(htmlView);

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            // Log the error or handle it (throw or ignore depending on your app needs)
            Console.WriteLine($"Error sending email: {ex.Message}");
            // Or rethrow if you want the caller to handle it
            // throw;
        }
    }




}
