using System.Text.Json;

namespace Chat_Server
{
	public class Message(string text, DateTime dateTime, string nickNameFrom, string nickNameTo)
	{
		public string Text { get; } = text;
		public DateTime DateTime { get; } = dateTime;
		public string NickNameFrom { get; } = nickNameFrom;
		public string NickNameTo { get; } = nickNameTo;

		public string SerializeMessageToJson() => JsonSerializer.Serialize(this);
		public static Message? DeserializeJsonToMessage(string json) => JsonSerializer.Deserialize<Message>(json);

		public void Print()
		{
			Console.WriteLine($"{this.DateTime.ToLongTimeString()} from {this.NickNameFrom}: \"{this.Text}\"");
		}
	}
}
