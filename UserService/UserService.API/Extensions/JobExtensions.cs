using Hangfire;
using UserService.BLL.Interfaces.Jobs;

namespace UserService.API.Extensions
{
    public static class JobExtensions
    {
        public static void AddJobs(this IApplicationBuilder application, IConfiguration configuration)
        {
            application
                .ApplicationServices
                .GetService<IRecurringJobManager>()
                .AddOrUpdate<IUserJobs>(
                    nameof(IUserJobs.ClearNotActivatedAccountsAsync),
                    job => job.ClearNotActivatedAccountsAsync(),
                    configuration[$"Jobs:Recurring:ClearAccounts"]);
        }
    }
}
