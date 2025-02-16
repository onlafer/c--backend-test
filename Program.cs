var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Главная страница");
app.MapGet("/test", () => "Тестовый путь");
app.MapGet("/test/sub-test", () => "Тестовый саб путь");

app.Run();
