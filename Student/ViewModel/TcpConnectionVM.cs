using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Student.ViewModel
{
    public class TcpConnectionVM
    {
        TcpClient client;

        public bool Connect(string ip, int port, out string errorMessage)
        {
            client = new TcpClient();
            errorMessage = string.Empty;
            try
            {
                if (!IPAddress.TryParse(ip, out var address))
                    return false;
                client.Connect(address, port);
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                return false;
            }
        }

        public bool IsIPValid(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            var reg = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            var match = reg.Match(ip);
            return match.Success;
        }

        public void SendUsername(string username)
        {
        }
    }
}