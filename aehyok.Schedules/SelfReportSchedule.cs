using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.Schedules
{
    public class SelfReportSchedule : CronScheduleService
    {
        protected override Task ProcessAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("实现自主填报的功能");
            return Task.CompletedTask;
        }
    }
}
