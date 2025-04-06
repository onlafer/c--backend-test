var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy => 
    {
        policy.WithOrigins("http://localhost:3000")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.MapControllers();

app.Run();