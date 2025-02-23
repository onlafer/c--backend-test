using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<IMessageService, MessageService>();
                services.AddTransient<App>();
            })
            .Build();

        var app = host.Services.GetRequiredService<App>();
        app.Run();
    }
}
