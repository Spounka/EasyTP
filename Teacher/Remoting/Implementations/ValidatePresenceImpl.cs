using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using SharedLib;
using SharedLib.Models;
using Teacher.Model;

namespace Teacher.Remoting.Implementations
{
    public class ValidatePresenceImpl : MarshalByRefObject, IPresence
    {
        public static event Action<UserModel> UserValidated;

        public static ConcurrentDictionary<UserModel, ConcurrentBag<StateObject>> ConnectUsers { get; } =
            new ConcurrentDictionary<UserModel, ConcurrentBag<StateObject>>();

        public void ValidatePresence(object dataObject, TcpClient handle)
        {
            var userModel = (UserModel)dataObject;
            if (ConnectUsers.Any(s =>
                    string.Compare(s.Key.MacAddress, userModel.MacAddress, StringComparison.Ordinal) > -1))
                return;
            var state = new StateObject()
            {
                handler = handle,
            };
            ConnectUsers[userModel] = new ConcurrentBag<StateObject>();
            OnUserValidated(userModel);
        }

        private static void OnUserValidated(UserModel userModel)
        {
            if (userModel != null)
                UserValidated?.Invoke(userModel);
        }
    }
}