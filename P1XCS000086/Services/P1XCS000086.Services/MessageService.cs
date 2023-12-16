using P1XCS000086.Services.Interfaces;

namespace P1XCS000086.Services
{
	public class MessageService : IMessageService
	{
		public string GetMessage()
		{
			return "Hello from the Message Service";
		}
	}
}
