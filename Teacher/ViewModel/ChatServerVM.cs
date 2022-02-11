using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedLib.Models;

namespace Teacher.ViewModel
{
    public class ChatServerVM : INotifyPropertyChanged
    {
        public ObservableCollection<StudentModel> Students { get; } = new ObservableCollection<StudentModel>();

        public string ServerStatus => "Status: " + (IsServerActive ? "Connected" : "Offline");

        public bool IsServerActive { get; private set; }
        private string WindowWidth { get; set; }
        private string WindowHeight { get; set; }


        private Task listeningTask;

        private TcpListener Server;
        private CancellationTokenSource _taskSource;
        private CancellationToken _ct;

        public async void StartListening(int port)
        {
            try
            {
                Server = new TcpListener(IPAddress.Any, port);
                Server.Start();

                IsServerActive = true;
                OnPropertyChanged("ServerStatus");
                OnPropertyChanged("IsServerActive");

                Students.Add(new StudentModel()
                {
                    FullName = "Dummy Name",
                    Group = "Dummy Group"
                });

                _taskSource = new CancellationTokenSource();
                _ct = _taskSource.Token;
                listeningTask = Task.Run(ListeningTaskAction, _taskSource.Token);
                try
                {
                    await listeningTask;
                }
                catch (OperationCanceledException)
                {
                    //abort
                }
                finally
                {
                    _taskSource.Dispose();
                    listeningTask.Dispose();
                }
            }
            catch (SocketException)
            {
                //abort
            }
        }

        private void ListeningTaskAction()
        {
            _ct.ThrowIfCancellationRequested();
            try
            {
                while (true)
                {
                    var handler = Server.AcceptTcpClient();
                    var stream = handler.GetStream();

                    var bytes = new byte[30];
                    var bytesRead = stream.Read(bytes, 0, bytes.Length);
                    if (bytesRead < 1)
                        break;
                    else
                        Students.Add(new StudentModel() { FullName = Encoding.UTF8.GetString(bytes), Group = "01" });
                    if (_ct.IsCancellationRequested)
                    {
                        Server.Stop();
                        _ct.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (SocketException)
            {
            }
        }

        public void StopServer()
        {
            _taskSource.Cancel();
            Server.Stop();
            IsServerActive = false;
            OnPropertyChanged("ServerStatus");
            OnPropertyChanged("IsServerActive");
        }

        public void OnWindowResize(double newSizeHeight, double newSizeWidth)
        {
            WindowHeight = $"Height: {newSizeHeight}";
            WindowWidth = $"Width: {newSizeWidth}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}