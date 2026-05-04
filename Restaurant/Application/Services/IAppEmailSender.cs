namespace Restaurant.Application.Services;

public interface IAppEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
