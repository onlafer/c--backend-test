using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var env = hostingContext.HostingEnvironment;
        env.EnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
        config.AddCommandLine(args);
        config.AddJsonFile("customconfig.json", optional: true, reloadOnChange: true);
    })
    .Build();

var configuration = host.Services.GetRequiredService<IConfiguration>();

Console.WriteLine($"Приложение запущено в среде: {configuration.GetValue<string>("Environment") ?? "Development"}");
Console.WriteLine($"Название приложения: {configuration["AppSettings:AppName"]}");
Console.WriteLine($"Версия: {configuration["AppSettings:Version"]}");
Console.WriteLine($"Строка подключения: {configuration["AppSettings:DefaultConnection"]}");
Console.WriteLine($"Уровень логирования: {configuration["AppSettings:LogLevel"]}");
Console.WriteLine($"Расширенные функции включены: {configuration["AppSettings:Features:EnableAdvancedFeatures"]}");
Console.WriteLine($"Максимальное количество элементов: {configuration["AppSettings:Features:MaxItems"]}");
Console.WriteLine($"URL API: {configuration["ExternalServices:ApiUrl"]}");
Console.WriteLine($"Таймаут API (сек): {configuration["ExternalServices:Timeout"]}");

var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
var externalServices = configuration.GetSection("ExternalServices").Get<ExternalServices>();

Console.WriteLine("\nСтрого типизированная конфигурация:");
Console.WriteLine($"Название приложения: {appSettings.AppName}");
Console.WriteLine($"Максимальное количество элементов: {appSettings.Features.MaxItems}");
Console.WriteLine($"Ключ API: {externalServices.ApiKey}");

public class AppSettings
{
    public string AppName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string DefaultConnection { get; set; } = string.Empty;
    public string LogLevel { get; set; } = string.Empty;
    public FeatureSettings Features { get; set; } = new FeatureSettings();
}

public class FeatureSettings
{
    public bool EnableAdvancedFeatures { get; set; }
    public int MaxItems { get; set; }
}

public class ExternalServices
{
    public string ApiUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int Timeout { get; set; }
}
