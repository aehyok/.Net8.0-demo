// See https://aka.ms/new-console-template for more information
using aehyok.Schedules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                //注册后台服务
                services.AddHostedService<CronScheduleService>();
            })
            .Build();

// 运行主机
await host.RunAsync();