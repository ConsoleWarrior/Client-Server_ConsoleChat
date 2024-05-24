﻿namespace Chat_Client
{
	public class Program
	{
		static void Main(string[] args)
		{
			string name = "Anonymus";
			if (args.Length == 1) name = args[0];
			else { Console.WriteLine("Введите свое имя:"); name = Console.ReadLine(); }
			Client client = new Client(name);
			client.ConnectTo("127.0.0.1", 1234);
		}
	}
}
