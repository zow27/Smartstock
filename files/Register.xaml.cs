using System.IO;
using System.Windows;

namespace SmartStock
{
    public partial class Register : Window
    {
        private readonly string credentialsFile = "credentials.txt";

        public Register()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameInput.Text.Trim();
            string password = PasswordInput.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Save credentials to a local file (very basic storage)
            File.WriteAllText(credentialsFile, $"{username}|{password}");

            MessageBox.Show("Registration successful!");
            this.Close();
        }
    }
}
