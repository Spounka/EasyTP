using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
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

        private void OnConnectButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!_connectionVm.IsIPValid(IPField.Text))
            {
                MessageBox.Show("IP / Host invalid");
                return;
            }

            if (!int.TryParse(PortField.Text, out var port))
            {
                MessageBox.Show("IP / Host invalid");
                return;
            }

            if (!_connectionVm.Connect(IPField.Text, port, out var errorMessage))
            {
                MessageBox.Show($"Error connecting to server: {errorMessage}");
                return;
            }

            if (string.IsNullOrWhiteSpace(FullNameField.Text) || string.IsNullOrEmpty(FullNameField.Text))
            {
                MessageBox.Show("FullName cannot be empty");
                return;
            }

            _connectionVm.SendUsername(FullNameField.Text);
        }

        private void OnDisconnectButtonClicked(object sender, RoutedEventArgs e)
        {
        }
    }
}