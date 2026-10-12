using System.Windows;
using CreditApp.Pages.Client;

namespace CreditApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();

            if (App.CurrentClient != null)
            {
                userText.Text = App.CurrentClient.secondname + " " + App.CurrentClient.firstname;
            }
            else if (App.CurrentUser != null)
            {
                userText.Text = App.CurrentUser.login;
            }

            mainFrame.Navigate(new AccountsPage());
        }

        private void accountsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new AccountsPage());
        }

        private void plansBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new CreditPlansPage());
        }

        private void requestsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new RequestsPage());
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            App.CurrentClient = null;
            App.isAdmin = false;
            new Authorization().Show();
            this.Close();
        }
    }
}
