using System.Windows;
using CreditApp.Pages.Admin;

namespace CreditApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            if (App.CurrentUser != null)
            {
                userText.Text = App.CurrentUser.login;
            }

            mainFrame.Navigate(new RequestsManagePage());
        }

        private void requestsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new RequestsManagePage());
        }

        private void usersBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new UsersPage());
        }

        private void plansBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new CreditPlansManagePage());
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
