using Microsoft.Extensions.Logging;

namespace Restaurant.Application.Services;

public class AppEmailSender : IAppEmailSender
{
    private readonly ILogger<AppEmailSender> _logger;

    public AppEmailSender(ILogger<AppEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("=== START EMAIL ===");
        _logger.LogInformation("To: {Email}", email);
        _logger.LogInformation("Subject: {Subject}", subject);
        _logger.LogInformation("Body: {Body}", htmlMessage);
        _logger.LogInformation("=== END EMAIL ===");

        return Task.CompletedTask;
    }
}
