using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
namespace Discount.api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailibility = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var _configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("migration postgres database");
                    using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection,
                    };

                    command.CommandText = "drop table if exists Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"Create table Coupon(ID serial primary key,
                                            ProductName varchar(200) not null,
                                            Description text,
                                             Amount int )";
                    command.ExecuteNonQuery();

                    command.CommandText = @"insert into Coupon(ProductName,Description,Amount) values('asus','asus des',65456)";
                    command.ExecuteNonQuery();
                    command.CommandText = @"insert into Coupon(ProductName,Description,Amount) values('asus 2','asus des 2',65456)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("migration complated");
                }
                catch (Exception e)
                {
                    logger.LogError("an error");
                    if (retryForAvailibility < 50)
                    {
                        retryForAvailibility++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailibility);
                    }
                }
                return host;
            }
        }
    }
}
