using System.IO;

using Microsoft.AspNetCore.Hosting;

namespace Infinni.Northwind
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                    .UseUrls("http://localhost:9900")
                    .UseKestrel()
                    .UseStartup<Startup>()
                    .Build();

            host.Run();
        }
    }
}