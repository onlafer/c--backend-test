using LoggingDemo.Models;
using LoggingDemo;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFile("Logs/myapp-{Date}.txt");
builder.Logging.AddProvider(new PostgreSqlLoggerProvider(null, builder.Services.BuildServiceProvider()));

var app = builder.Build();

app.MapControllers();

app.Run();
