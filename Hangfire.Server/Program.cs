using Microsoft.Owin.Hosting;

namespace Hangfire.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:1234"))
            {
                System.Console.WriteLine("Hangfire is running");
                System.Console.WriteLine("Press any key to stop");
                System.Console.ReadKey();
            }
        }
    }
}
