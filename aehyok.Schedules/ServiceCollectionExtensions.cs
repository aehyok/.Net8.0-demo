using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using aehyok.Infrastructure.TypeFinders;
namespace aehyok.Schedules
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCronServices(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            var cronType = typeof(CronScheduleService);

            //foreach (Assembly assembly in assemblies)
            //{
            //    foreach(var type in assembly.GetTypes())
            //    {
            //        //判断type是否继承了CronScheduleService类
            //        if(type.IsAssignableTo(cronType) && !type.IsAbstract)
            //        {
            //            services.Add(new ServiceDescriptor(typeof(IHostedService), type, ServiceLifetime.Singleton));
            //        }
            //    }
            //}

            foreach (var type in TypeFinders.SearchTypes(cronType, false))
            {
                services.Add(new ServiceDescriptor(typeof(IHostedService), type, ServiceLifetime.Singleton));
            }
            return services;
        }
    }
}
