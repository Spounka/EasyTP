using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Teacher.Remoting.Implementations;

namespace Teacher.Remoting
{
    public static class RemotingServer
    {
        public static void StartRemotingServer(int port)
        {
            var channel = new TcpChannel(port);

            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ValidatePresenceImpl), "presence",
                WellKnownObjectMode.Singleton);
        }
    }
}