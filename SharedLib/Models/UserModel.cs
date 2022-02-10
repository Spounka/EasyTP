using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SharedLib.Models
{
    [System.Serializable]
    public class UserModel
    {
        public string FullName { get; set; }
        public string Group { get; set; }
        public TcpClient handler { get; set; }
        public readonly string MacAddress;

        public UserModel()
        {
            MacAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(s => s.OperationalStatus == OperationalStatus.Up)
                .Aggregate("", (s, e) => e.GetPhysicalAddress().ToString());
        }
    }
}