using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Configuration.Loaders;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Messaging.Tests;

public class MailgunEmailServiceTests
{
    [Fact]
    public async Task Should_SendEmail()
    {
        var configLoader = new ConfigurationLoader(nameof(EnvironmentType.Local));
        configLoader.LoadEnvironment();

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<MailgunEmailService>();

        var emailService = new MailgunEmailService(logger);
        var to = Environment.GetEnvironmentVariable("TEST_EMAIL_TO");
        const string subject = "Test Email";
        var body = $"Test email \n \n Timestamp: {DateTime.UtcNow} \n \n Mailgun email service working!";

        var response = await emailService.SendEmailAsync(to!, subject, body);

        Assert.True(response.Success);
    }
}
