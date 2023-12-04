// See https://aka.ms/new-console-template for more information
using aehyok.Infrastructure;
using aehyok.Schedules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using aehyok.RabbitMQ;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);


builder.Host.InitHost("aehyok.Schedules");

builder.Services.AddRabbitMQ(builder.Configuration);


builder.Services.AddScoped<IRabbitMQConnection, RabbitMQConnection>();
builder.Services.AddScoped<ICF, FanoutCF>();



var icf = builder.Services.BuildServiceProvider().GetRequiredService<ICF>();


//icf.Subscrber();
//icf.Publish();

var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                //注册后台服务
                services.AddHostedService<SelfReportSchedule>();
                services.AddHostedService<QuestionSchedule2>();
            })
            .Build();

// 运行主机
await host.RunAsync();