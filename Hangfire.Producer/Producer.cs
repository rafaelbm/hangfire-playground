using Hangfire.Console;
using Hangfire.Producer.Filters;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hangfire.Producer
{
    public class Producer
    {
        //Mutex uses string.Format internaly
        [Mutex("{1}")]
        public async Task Produces1(PerformContext context, string resourceKey)
        {
            context.WriteLine("Doing something that requires a lock...");

            // Emulates some workload
            await Task.Delay(TimeSpan.FromSeconds(3));

            context.WriteLine("Long running task completed");
        }

        // Since FooArgs.ToString() will be used as the Mutex resource key this method will be executed sequentially
        // Another option is to override FooArgs.ToString() this way the Mutex resource key can be customized to use the class properties
        [Mutex("{0}")]
        public async Task Produces2(FooArgs args, PerformContext context)
        {
            context.WriteLine("Doing something that requires a lock...");

            // Emulates semi-long running operation
            await Task.Delay(TimeSpan.FromSeconds(3));

            context.WriteLine("Long running task completed");
        }
    }

    public class FooArgs
    {
        public int MyProperty1 { get; set; }
        public DateTime MyProperty2 { get; set; }
        public List<Bar> MyProperty3 { get; set; }

        public FooArgs()
        {
            MyProperty3 = new List<Bar>();
        }

        public override string ToString()
        {
            // Can be customized
            return base.ToString();
        }
    }

    public class Bar
    {
        public int MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
    }
}
