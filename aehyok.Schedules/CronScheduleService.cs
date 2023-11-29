using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.Schedules
{
    /// <summary>
    /// 实现一个后台服务
    /// </summary>
    public class CronScheduleService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Test CronSchedule");
            while(!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Thread.Sleep(1000);
            }
            return Task.CompletedTask;
        }
    }
}
