using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Console;

namespace Hangfire.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Hangfire")
                .UseConsole();

            var now = DateTime.UtcNow;
            var fooArgs = new FooArgs()
            {
                MyProperty1 = 1,
                MyProperty2 = now,
                MyProperty3 = new List<Bar>()
                {
                    new Bar() { MyProperty1 = 1, MyProperty2 = "1"}
                }
            };

            for (int i = 0; i < 5; i++)
            {
                BackgroundJob.Enqueue<Producer>(x => x.Produces1(null, $"SomeKey"));
                BackgroundJob.Enqueue<Producer>(x => x.Produces2(fooArgs, null));
            }

            System.Console.WriteLine("Enqueue completed");
            System.Console.ReadKey();
        }
    }
}
