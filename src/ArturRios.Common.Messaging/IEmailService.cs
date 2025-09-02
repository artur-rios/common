using ArturRios.Common.Output;

namespace ArturRios.Common.Messaging;

public interface IEmailService
{
    Task<ProcessOutput> SendEmailAsync(string to, string subject, string body);
}
