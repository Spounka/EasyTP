using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SharedLib.Models;
using Teacher.Model;
using System.Windows.Data;
using Teacher.Remoting.Implementations;

namespace Teacher.ViewModel
{
    public class ChatServerVM : INotifyPropertyChanged
    {
        public BindingList<UserModel> Students { get; } = new BindingList<UserModel>();
        private readonly object studentsLock = new object();


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
            ValidatePresenceImpl.UserValidated += AddUser;

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

            BindingOperations.EnableCollectionSynchronization(Students, studentsLock);

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

                    var task = new Task(() => StartReadTask(handler), TaskCreationOptions.AttachedToParent);
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

        private void AddUser(UserModel userModel)
        {
            Students.Add(userModel);
        }


        private void StartReadTask(TcpClient handler)
        {
            var stream = handler.GetStream();
            var messageSizeBytes = new byte[4];

            while (handler.Connected && IsServerActive)
            {
                try
                {
                    var state = new StateObject()
                    {
                        handler = handler
                    };
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (IOException)
                {
                    break;
                }
            }

            Debug.WriteLine("Disconnected");
        }

        private object ReadObject(StateObject stateObject)
        {
            var handler = stateObject.handler;
            var stream = handler.GetStream();
            var formatter = new SoapFormatter();
            return formatter.Deserialize(stream);
        }

        public async Task WriteAsync(UserModel model, StateObject state)
        {
            var stream = state.handler.GetStream();
            var messageLength = state.Buffer.Length;
            var messageLengthBytes = Encoding.UTF8.GetBytes(messageLength.ToString());

            stream.Write(messageLengthBytes, 0, messageLength);
            await stream.WriteAsync(state.Buffer, 0, StateObject.BufferSize, _ct);
        }

        public void BroadCast(string message)
        {
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