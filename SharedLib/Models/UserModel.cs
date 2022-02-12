using System.Linq;
using System.Net.NetworkInformation;

namespace SharedLib.Models
{
    [System.Serializable]
    public class UserModel
    {
        public string FullName { get; set; }
        public string Group { get; set; }

        public string MacAddress { get; private set; }

        public UserModel()
        {
            MacAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(s => s.OperationalStatus == OperationalStatus.Up)
                .Aggregate("", (s, e) => s += e.GetPhysicalAddress().ToString());
        }
    }
}