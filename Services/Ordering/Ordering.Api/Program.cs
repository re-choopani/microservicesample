
using AutoMapper;
using EventBus.Messages.Common;
using MassTransit;
using Ordering.Api.EventBusConsumer;
using Ordering.Api.Extensions;
using Ordering.Api.Mapping;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Ordering.Application;
using Autofac.Core;
using System.Reflection;

namespace Ordering.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<BasketCheckoutConsumer>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //var mapperConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new OrderingProfile());
            //});
            //IMapper mapper = mapperConfig.CreateMapper();
            //builder.Services.AddSingleton(mapper);
            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketCheckoutConsumer>();
                config.UsingRabbitMq((ctx, conf) =>
                {
                    conf.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
                    conf.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, x =>
                    {
                        x.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                    });
                });
            });
            builder.Services.Configure<MassTransitHostOptions>(options =>
            {
                options.WaitUntilStarted = true;
                options.StartTimeout = TimeSpan.FromSeconds(30);
                options.StopTimeout = TimeSpan.FromMinutes(1);
            });
            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.MigrationDatabase<OrderContext>((context, services) =>
            {
                var logger = app.Services.GetService<ILogger<OrderContextSeed>>();
                logger.LogInformation($"{nameof(OrderContextSeed)}");
                OrderContextSeed.SeedAsync(context, logger).Wait();
            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
