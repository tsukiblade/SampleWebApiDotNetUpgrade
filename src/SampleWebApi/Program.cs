using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Models;

namespace SampleWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}