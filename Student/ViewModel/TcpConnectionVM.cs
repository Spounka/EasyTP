using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Student.Model;

namespace Student.ViewModel
{
    public class TcpConnectionVM
    {
        private TcpClient client;
        public StudentConnectionModel _connectionModel { get; set; }

        public async Task Connect()
        {
            client = new TcpClient();
            try
            {
                await client.ConnectAsync(_connectionModel.ServerIP, _connectionModel.Port);
                await SendUsername();
            }
            catch (SocketException)
            {
                Console.WriteLine("Error, Socket unavailable");
            }
        }

        private Task SendUsername()
        {
            var usernameBytes = Encoding.UTF8.GetBytes(_connectionModel.FullName);
            var stream = client.GetStream();
            return stream.WriteAsync(usernameBytes, 0, usernameBytes.Length);
        }
    }
}