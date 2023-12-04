using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Reflection;
using Microsoft.Extensions.Hosting;
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

            foreach (Assembly assembly in assemblies)
            {
                foreach(var type in assembly.GetTypes())
                {
                    if(type.IsAssignableTo(cronType) && !type.IsAbstract)
                    {
                        services.Add(new ServiceDescriptor(typeof(IHostedService), type, ServiceLifetime.Singleton));
                    }
                }
            }


            //cronServices.ForEach(a =>
            //{
            //    services.Add(new ServiceDescriptor(typeof(IHostedService), a, ServiceLifetime.Singleton));
            //});

            return services;
        }
    }
}
