namespace ConsoleApp
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Тестовое сообщение";
        }
    }
}