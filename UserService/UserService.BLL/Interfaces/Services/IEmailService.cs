using UserService.BLL.Common.Dtos;

namespace UserService.BLL.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto request, CancellationToken cancellationToken = default);
    }
}
