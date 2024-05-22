using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Server
{
	public class Server
	{
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
				while (flag)
				{
					var json = reader.ReadLine();
					if (json == null) break;
					var message = Message.DeserializeJsonToMessage(json);
					clientName = message.NickNameFrom;
					message.Print();
					var response = new Message("Сообщение успешно доставлено", DateTime.Now, "Server", message.NickNameFrom);
					writer.WriteLine(response.SerializeMessageToJson());
					writer.Flush();
				}
			}
			catch (Exception e) { Console.WriteLine(e.Message); }
			Console.WriteLine($"{clientName} connection lost");
		}

		private void Stop()
		{
			while (true)
			{
				if (Console.ReadLine() == "exit")
				{
					flag = false;
					return;
				}
			}
		}
	}
}

