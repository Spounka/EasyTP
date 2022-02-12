using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SharedLib.Models;
using Teacher.Model;

namespace Teacher.ViewModel
{
    public class ChatServerVM : INotifyPropertyChanged
    {
        public BindingList<UserModel> Students { get; } = new BindingList<UserModel>();

        private ConcurrentDictionary<UserModel, ConcurrentBag<StateObject>> something =
            new ConcurrentDictionary<UserModel, ConcurrentBag<StateObject>>();

        public string ServerStatus => $"Status:  {(IsServerActive ? "Connected" : "Offline")} ";

        public bool IsServerActive { get; private set; }

        private TcpListener Server;
        private CancellationTokenSource _taskSource;
        private CancellationToken _ct;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CreateServer(int port)
        {
            // Initialize the Cancellation Token
            _taskSource = new CancellationTokenSource();
            _ct = _taskSource.Token;

            // Initialize the Server
            InitServer(port);
            NotifyPropertyChanges();
        }

        private void NotifyPropertyChanges()
        {
            // Notifies the UI about the changes
            OnPropertyChanged("ServerStatus");
            OnPropertyChanged("IsServerActive");
        }

        private void InitServer(int port)
        {
            Server ??= new TcpListener(IPAddress.Any, port);
            Server.Start();
            IsServerActive = true;

            // Starts listening
            var task = new Task(ListeningTask);
            task.Start();
        }

        private void ListeningTask()
        {
            try
            {
                while (!_ct.IsCancellationRequested && IsServerActive)
                {
                    _ct.ThrowIfCancellationRequested();

                    var handler = Server.AcceptTcpClient();
                    var userModel = new UserModel()
                    {
                        handler = handler
                    };

                    something[userModel] = new ConcurrentBag<StateObject>();
                    var task = new Task(() => StartReadTask(userModel), TaskCreationOptions.AttachedToParent);
                    task.Start();
                }
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException e)
            {
                MessageBox.Show("haha, Error still here" + e);
            }
            finally
            {
                Server.Stop();
                _taskSource.Dispose();
                Server = null;
            }
        }

        private void StartReadTask(UserModel userModel)
        {
            var handler = userModel.handler;
            while (handler.Connected && IsServerActive)
            {
                var stream = handler.GetStream();
                var state = new StateObject();

                var messageSizeBytes = new byte[4];
                var bytesRead = stream.Read(messageSizeBytes, 0, messageSizeBytes.Length);
                if (bytesRead <= 0) continue;

                if (!int.TryParse(Encoding.UTF8.GetString(messageSizeBytes), out var messageSize)) continue;

                stream.Read(state.Buffer, 0, messageSize);
                something[userModel].Add(state);
            }
        }

        public async Task WriteAsync(UserModel model, StateObject state)
        {
            var stream = model.handler.GetStream();
            var messageLength = state.Buffer.Length;
            var messageLengthBytes = Encoding.UTF8.GetBytes(messageLength.ToString());

            stream.Write(messageLengthBytes, 0, messageLength);
            await stream.WriteAsync(state.Buffer, 0, StateObject.BufferSize, _ct);
        }


        public void StopServer()
        {
            if (IsServerActive && _ct.CanBeCanceled)
                _taskSource.Cancel();

            IsServerActive = false;
            NotifyPropertyChanges();
        }
    }
}