﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sync_client
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8080; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                EndPoint remoteIpPoint = new IPEndPoint(IPAddress.Any, 0);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                string message = "";
                while (message != "end")
                {
                    Console.Write("Enter a message:");
                    message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.SendTo(data, ipPoint);

                    // при використанні UDP протоколу, Connect() лише встановлює дані для відправки
                    //socket.Connect(ipPoint);
                    //socket.Send(data);

                    // получаем ответ
                    // получаем сообщение
                    int bytes = 0;
                    string response = "";
                    data = new byte[1024];
                    do
                    {
                        bytes = socket.ReceiveFrom(data, ref remoteIpPoint);
                        response += Encoding.Unicode.GetString(data, 0, bytes);
                    } while (socket.Available > 0);

                    Console.WriteLine("server response: " + response);
                }
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
