using System;
using System.IO;
using System.Threading.Tasks;
using Chroniton;
using Chroniton.Schedules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mrtn.Data;
using Serilog;

namespace Mrtn
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            InitializeLogger(loggerFactory, env, appLifetime);

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvc(routes => { });

            Storage.Initialize(Path.Combine(env.ContentRootPath, "db"));

            var singularity = Singularity.Instance;
            var job = new AsyncJob(MrtnMonitor.RunAsync);
            var schedule = new EveryXTimeSchedule(TimeSpan.FromHours(1));
            singularity.ScheduleJob(schedule, job, true);
            singularity.Start();
        }

        private static void InitializeLogger(ILoggerFactory loggerFactory, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            var filename = Path.Combine(env.ContentRootPath, "logs", "log.txt");

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile(filename, retainedFileCountLimit: 1)
                .CreateLogger();

            loggerFactory.AddSerilog();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }

        private sealed class AsyncJob : IJob
        {
            private readonly Func<Task> _action;

            public AsyncJob(Func<Task> action)
            {
                _action = action;
            }

            public string Name { get; set; } = "async_job";

            public ScheduleMissedBehavior ScheduleMissedBehavior => ScheduleMissedBehavior.RunAgain;

            public Task Start(DateTime scheduledTime) => _action();
        }
    }
}
