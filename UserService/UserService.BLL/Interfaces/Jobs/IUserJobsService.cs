using UserService.DAL.Entities;

namespace UserService.BLL.Interfaces.Jobs
{
    public interface IUserJobsService
    {
        Task ClearNotActivatedAccountsAsync();
        Task SendActivateCode(string email, int activateCode);
    }
}
