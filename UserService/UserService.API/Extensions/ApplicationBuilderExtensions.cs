using Hangfire;
using UserService.BLL.Interfaces.Jobs;

namespace UserService.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddJobs(this IApplicationBuilder application, IConfiguration configuration)
        {
            application
                .ApplicationServices
                .GetService<IRecurringJobManager>()
                .AddOrUpdate<IUserJobsService>(
                    nameof(IUserJobsService.ClearNotActivatedAccountsAsync),
                    job => job.ClearNotActivatedAccountsAsync(),
                    configuration[$"Jobs:Recurring:ClearAccounts"]);
        }
    }
}
