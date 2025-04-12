using Hangfire;
using UserService.BLL.Interfaces.Jobs;
using UserService.BLL.Jobs;
using UserService.DAL.Interfaces;

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

            for (int i = 0; i < 5; i++)
            {
                application
                .ApplicationServices
                .GetService<IRecurringJobManager>()
                .AddOrUpdate<OutboxMessageJobs>(
                    nameof(OutboxMessageJobs.ProcessOutboxMessagesJob),
                    job => job.ProcessOutboxMessagesJob(),
                    configuration[$"Jobs:Recurring:ProcessOutbox"]);
            }
        }

        public static void UseApplyMigrations(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            unitOfWork.DatabaseMigrate();
        }
    }
}
