using System.Windows;
using Teacher.ViewModel;

namespace Teacher.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ChatServerVM vm;

        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += OnWindowResize;
            vm = new ChatServerVM();
            DataContext = vm;
        }

        private void OnWindowResize(object sender, SizeChangedEventArgs e)
        {
            vm.OnWindowResize(e.NewSize.Height, e.NewSize.Width);
        }

        private void StartServer(object sender, RoutedEventArgs e)
        {
            var portNumberText = PortNumberTextBox.Text;
            if (string.IsNullOrEmpty(portNumberText) || string.IsNullOrWhiteSpace(portNumberText))
                portNumberText = PortNumberTextBox.Text = "8000";

            var success = int.TryParse(portNumberText, out var portNumber);
            if (!success)
                MessageBox.Show("Error, Input a valid Port number!", "EasyTP - Teacher");
            else
                vm.StartListening(portNumber);
        }

        private void StopServer(object sender, RoutedEventArgs e)
        {
            vm.StopServer();
        }
    }
}