
using AutoMapper;
using Basket.Api.GrpcServices;
using Basket.Api.Mapper;
using Basket.Api.Repositories;
using DiscountGrpc.Protos;
using MassTransit;
using StackExchange.Redis;

namespace Basket.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BasketProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, conf) =>
                {
                    conf.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
                });
            });
            builder.Services.Configure<MassTransitHostOptions>(options =>
            {
                options.WaitUntilStarted = true;
                options.StartTimeout = TimeSpan.FromSeconds(30);
                options.StopTimeout = TimeSpan.FromMinutes(1);
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(option =>
            {
                option.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")??throw new Exception("Configration Not found"));
            }).ConfigurePrimaryHttpMessageHandler<MyHttpClientHandler>();
            builder.Services.AddScoped<DiscountGrpcService>();
            builder.Services.AddSingleton<MyHttpClientHandler>();
            var app = builder.Build();

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
