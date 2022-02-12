using System.Net.Sockets;
using System.Text;

namespace Teacher.Model
{
    [System.Serializable]
    public class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];

        public override string ToString()
        {
            return Encoding.UTF8.GetString(Buffer);
        }
    }
}