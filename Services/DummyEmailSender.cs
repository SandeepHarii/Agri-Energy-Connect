using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine("=== Dummy Email Sent ===");
            Console.WriteLine($"To: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {htmlMessage}");
            Console.WriteLine("========================");
            return Task.CompletedTask;
        }
    }
}
