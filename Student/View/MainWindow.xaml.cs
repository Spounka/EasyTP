using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Student.Model;
using Student.ViewModel;

namespace Student.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private TcpConnectionVM _connectionVm;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _connectionVm = new TcpConnectionVM();
        }

        private async void OnConnectButtonClicked(object sender, RoutedEventArgs e)
        {
            var binding = BindingOperations.GetBinding(IPField, TextBox.TextProperty);
            if (binding == null) return;

            _connectionVm._connectionModel = (StudentConnectionModel)binding.Source;
            await _connectionVm.Connect();
        }

        private void OnDisconnectButtonClicked(object sender, RoutedEventArgs e)
        {
        }
    }
}