using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace aehyok.Infrastructure
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder InitHost(this IHostBuilder builder, string moduleKey)
        {
            Thread.CurrentThread.Name = moduleKey;
            
            //加载配置文件
            builder.ConfigureAppConfiguration((context, options) => 
            {
                var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                options.AddJsonFile( path, true, true);
            });

            return builder;
        }

    }
}
