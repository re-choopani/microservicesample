using Discount.Grpc.Extensions;
using Discount.Grpc.Repositories;
using DiscountGrpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace DiscountGrpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddGrpc();
            builder.Services.AddGrpcReflection();
            builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
            var app = builder.Build();
            //app.MigrateDatabase<Program>();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<DiscountService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}