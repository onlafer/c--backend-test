namespace ConsoleApp
{
    public class App
    {
        private readonly IMessageService _messageService;

        public App(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Run()
        {
            Console.WriteLine(_messageService.GetMessage());
        }
    }
}
