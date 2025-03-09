namespace TestApp.Services
{
    public class GreetingService : IGreetingService
    {
        public void Greet(string name)
        {
            Console.WriteLine($"Привет {name}!");
        }
    }
}