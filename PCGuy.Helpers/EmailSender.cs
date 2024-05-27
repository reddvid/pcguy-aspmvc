using Microsoft.AspNetCore.Identity.UI.Services;

namespace PCGuy.Helpers;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // TODO: Add logic to send email
        
        return Task.CompletedTask;
    }
}