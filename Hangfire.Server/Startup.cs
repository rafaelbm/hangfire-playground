using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.Server;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(Startup))]
namespace Hangfire.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangFire")
                .UseDashboardMetric(DashboardMetrics.ServerCount)
                .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                .UseDashboardMetric(DashboardMetrics.RetriesCount)
                .UseDashboardMetric(DashboardMetrics.EnqueuedCountOrNull)
                .UseDashboardMetric(DashboardMetrics.FailedCountOrNull)
                .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
                .UseDashboardMetric(DashboardMetrics.ScheduledCount)
                .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                .UseDashboardMetric(DashboardMetrics.SucceededCount)
                .UseDashboardMetric(DashboardMetrics.FailedCount)
                .UseDashboardMetric(DashboardMetrics.DeletedCount)
                .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                .UseConsole();

            app.UseHangfireDashboard("/hangfire");

            var options = new BackgroundJobServerOptions()
            {
                WorkerCount = 10
            };

            app.UseHangfireServer(options);
        }
    }
}
