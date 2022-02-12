using System.Net.Sockets;
using System.Text;
using SharedLib.Models;

namespace Teacher.Model
{
    public class StateObject
    {
        public const int BufferSize = 4096;
        public byte[] Buffer = new byte[BufferSize];
        public TcpClient handler;

        public override string ToString()
        {
            return Encoding.UTF8.GetString(Buffer);
        }
    }
}