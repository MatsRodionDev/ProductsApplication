using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using UserService.BLL.Common;
using UserService.BLL.Interfaces.Services;
using MailKit.Net.Smtp;
using UserService.BLL.Common.Dtos;

namespace UserService.BLL.Services
{
    public class EmailService(
        IOptions<EmailOptions> options,
        ILogger<EmailService> logger) : IEmailService
    {
        private readonly EmailOptions _options = options.Value;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task SendEmail(EmailDto request, CancellationToken cancellationToken = default)
        {
            var email = GetMimeMessage(request);

            using var smtpClient = new SmtpClient();
            smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
            try
            {
                await smtpClient.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
                await smtpClient.AuthenticateAsync(_options.From, _options.Password, cancellationToken);
                await smtpClient.SendAsync(email, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private MimeMessage GetMimeMessage(EmailDto request)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_options.From));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Body
            };

            return email;
        }
    }
}
