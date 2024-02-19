using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
namespace OcolotApiGateways
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile($"Ocelot.{builder.Environment.EnvironmentName}.json", optional: true);
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
            var app = builder.Build();
            await app.UseOcelot();

            app.MapGet("/", () => "Hello World!");

            await app.RunAsync();
        }
    }
}
