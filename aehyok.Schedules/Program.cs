// See https://aka.ms/new-console-template for more information
using aehyok.Infrastructure;
using aehyok.Schedules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using aehyok.RabbitMQ;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

builder.Host.InitHost("aehyok.Schedules");

builder.Services.AddRabbitMQ(builder.Configuration);

builder.Services.AddScoped<IRabbitMQConnection, RabbitMQConnection>();
builder.Services.AddScoped<ICF, FanoutCF>();

var icf = builder.Services.BuildServiceProvider().GetRequiredService<ICF>();

//icf.Subscrber();
//icf.Publish();
builder.Services.AddCronServices();

var app = builder.Build();

// 运行主机
await app.RunAsync();