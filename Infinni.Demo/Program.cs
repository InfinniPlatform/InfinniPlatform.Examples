using System.IO;

using Microsoft.AspNetCore.Hosting;

namespace Infinni.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                    .UseUrls("http://0.0.0.0:5000")    // workaround
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .Build();

            host.Run();
        }
    }
}