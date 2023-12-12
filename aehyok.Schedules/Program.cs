// See https://aka.ms/new-console-template for more information
using aehyok.Infrastructure;
using aehyok.Schedules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using aehyok.RabbitMQ;
using aehyok.RabbitMQ.EventBus;
using aehyok.Schedules.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using aehyok.EntityFramework.DbContexts;
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

Thread.CurrentThread.Name = "aehyok.Schedules";
//builder.Host.InitHost("aehyok.Schedules");


builder.Services.AddMysql(builder.Configuration);

builder.Services.AddRabbitMQ(builder.Configuration);

builder.Services.AddCronServices();

var app = builder.Build();

app.AddRabbitMQEventBus();

var service = app.Services.GetRequiredService<IEventPublisher>();
service.Publish(new SelfReportPublishEvent()
{
    TaskId = 111111
});

// 运行主机
await app.RunAsync();
