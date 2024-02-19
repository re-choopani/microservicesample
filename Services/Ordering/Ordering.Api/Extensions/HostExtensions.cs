using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrationDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    logger.LogInformation("start migration");
                    InvokeSeeder(seeder, context, services);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "error occurred");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrationDatabase<TContext>(host, seeder, retryForAvailability);
                    }
                    throw;
                }
            }
            return host;
        }
        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
            TContext context,
            IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
