using System.Net.Sockets;

namespace SharedLib
{
    public interface IPresence
    {
        void ValidatePresence(object dataObject, TcpClient handle);
    }
}