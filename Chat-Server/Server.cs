using System.Net.Sockets;
using System.Net;

namespace Chat_Server
{
	public class Server
	{
		List<StreamWriter> clientWriters = new();
		bool flag = true;

		public void Start()
		{
			TcpListener listener = new(IPAddress.Any, 1234);
			listener.Start();
			Task.Run(() => NewTcpClient(listener));
			Stop();
		}

		private void NewTcpClient(TcpListener listener)
		{
			Console.WriteLine("Waiting for connection...");
			string clientName = string.Empty;
			try
			{
				using var client = listener.AcceptTcpClient();
				Task.Run(() => NewTcpClient(listener));
				var reader = new StreamReader(client.GetStream());
				var writer = new StreamWriter(client.GetStream());
				clientWriters.Add(writer);
				while (flag)
				{
					var json = reader.ReadLine();
					if (json == null) break;
					var message = Message.DeserializeJsonToMessage(json);
					clientName = message.NickNameFrom;
					message.Print();
					//var response = new Message("Сообщение успешно доставлено", DateTime.Now, "Server", message.NickNameFrom);
					//writer.WriteLine(response.SerializeMessageToJson());
					//writer.Flush();
					SendToAll(writer, json);
				}
			}
			catch (Exception e)
			{ Console.WriteLine(e.Message); }
			Console.WriteLine($"{clientName} connection lost");
			SendToAll(null, new Message($"{clientName} connection lost", DateTime.Now, "Server", "All").SerializeMessageToJson());
		}

		private void SendToAll(StreamWriter? writer, string? json)
		{
			StreamWriter? disposedWriter = null;
			foreach (var item in clientWriters)
			{
				try
				{
					if (item.Equals(writer)) continue;
					item.WriteLine(json);
					item.Flush();
				}
				catch (ObjectDisposedException)
				{
					disposedWriter = item;
					Console.WriteLine("	***disposed***");
					continue;
				}
			}
			if (disposedWriter != null) clientWriters.Remove(disposedWriter);
		}

		private void Stop()
		{
			while (true)
				if (Console.ReadLine() == "exit")
				{
					flag = false;
					return;
				}
		}
	}
}

