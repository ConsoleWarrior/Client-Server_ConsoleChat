using Chat_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Chat_Client
{
	public class Client
	{
		private string name;
		private IPEndPoint endPoint;
		public Client(string name)
		{
			this.name = name;
		}

		public void ConnectTo(string ip, int port)
		{
			endPoint = new(IPAddress.Parse(ip), port);
			using (TcpClient client = new())
			{
				try
				{
					client.Connect(endPoint);
					Console.WriteLine("Connected to server");
					Run(client);
				}
				catch (SocketException)
				{
                    Console.WriteLine("Server not found");
                }


			}
		}
		public void Run(TcpClient client)
		{
			Task.Run(() => RecieveMessage(client));
			var writer = new StreamWriter(client.GetStream());

			var message = new Message($"{name} join to server!", DateTime.Now, name, "Server");
			writer.WriteLine(message.SerializeMessageToJson());
			writer.Flush();

			while (true)
			{
                Console.WriteLine("Input your message:");
                string inputText = Console.ReadLine();
				if (inputText == "exit") break;
				message = new Message(inputText, DateTime.Now, name, "Server");
				writer.WriteLine(message.SerializeMessageToJson());
				writer.Flush();

			}
		}
		public void SendMessage(TcpClient client, Message message)
		{
			var writer = new StreamWriter(client.GetStream());
			writer.WriteLine(message.SerializeMessageToJson());
			writer.Flush();
		}
		public void RecieveMessage(TcpClient client)
		{
			var reader = new StreamReader(client.GetStream());
			while (true)
			{
				var json = reader.ReadLine();
				if (json == null) break;
				var message = Message.DeserializeJsonToMessage(json);
				message.Print();
			}
		}
	}
}
