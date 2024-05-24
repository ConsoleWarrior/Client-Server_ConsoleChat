namespace Chat_Server
{
	public class Program
	{
		static void Main(string[] args)
		{
			Server server = new();
			server.Start();
			Console.WriteLine("SERVER DOWN");
		}
	}
}
