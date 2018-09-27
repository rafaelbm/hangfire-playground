using Hangfire.Common;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangfire.Producer.Filters
{
    public class CustomDisableConcurrentExecutionAttribute : JobFilterAttribute, IServerFilter
    {
        private readonly string _resource;

        public int TimeoutInSeconds { get; set; }

        public CustomDisableConcurrentExecutionAttribute(string resource)
        {
            _resource = resource;
            TimeoutInSeconds = 60;
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var resource = GetResource(filterContext.BackgroundJob.Job);

            var timeout = TimeSpan.FromSeconds(TimeoutInSeconds);

            var distributedLock = filterContext.Connection.AcquireDistributedLock(resource, timeout);
            filterContext.Items["DistributedLock"] = distributedLock;
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            if (!filterContext.Items.ContainsKey("DistributedLock"))
            {
                throw new InvalidOperationException("Can not release a distributed lock: it was not acquired.");
            }

            var distributedLock = (IDisposable)filterContext.Items["DistributedLock"];
            distributedLock.Dispose();
        }

        private string GetResource(Job job)
        {
            return $"extension:disable-concurrent:lock:{GetKeyFormat(job.Args)}";
        }

        private string GetKeyFormat(IEnumerable<object> args)
        {
            return string.Format(_resource, args.ToArray());
        }
    }
}
