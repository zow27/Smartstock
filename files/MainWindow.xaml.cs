using System.IO;
using System.Windows;

namespace SmartStock
{
    public partial class MainWindow : Window
    {
        private readonly string credentialsFile = "credentials.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        // This method name must match Click="LoginButton_Click" in XAML
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string inputUsername = UsernameBox.Text.Trim();
            string inputPassword = PasswordBox.Password.Trim();

            if (File.Exists(credentialsFile))
            {
                string[] creds = File.ReadAllText(credentialsFile).Split('|');
                string savedUsername = creds[0];
                string savedPassword = creds[1];

                if (inputUsername == savedUsername && inputPassword == savedPassword)
                {
                    // Open Dashboard and close login
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("No registered user found. Please register first.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // This method name must match Click="OpenRegister_Click" in XAML
        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.ShowDialog();
        }
    }
}
