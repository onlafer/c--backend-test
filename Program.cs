using Microsoft.Extensions.DependencyInjection;
using TestApp.Services;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IGreetingService, GreetingService>()
            .BuildServiceProvider();

        var greetingService = serviceProvider.GetService<IGreetingService>();

        Console.WriteLine("Введите ваше имя:");
        var name = Console.ReadLine();
        greetingService.Greet(name);
    }
}