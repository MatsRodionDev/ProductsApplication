using Microsoft.Extensions.Logging;
using UserService.BLL.Common.Dtos;
using UserService.BLL.Interfaces.Jobs;
using UserService.BLL.Interfaces.Services;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Jobs
{
    internal sealed class UserJobsService(
        IUnitOfWork unitOfWork,
        IEmailService emailService) : IUserJobsService
    {
        public async Task ClearNotActivatedAccountsAsync()
        {
            var users = await unitOfWork.UserRepository.GetNotActivatedUsersAsync(1, 20);

            foreach (var user in users)
            {
                unitOfWork.UserRepository.Delete(user);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task SendActivateCode(string email, int activateCode)
        {
            var dto = new EmailDto
            {
                To = email,
                Subject = "Activation code",
                Body = activateCode.ToString()
            };

            await emailService.SendEmail(dto);
        }
    }
}
