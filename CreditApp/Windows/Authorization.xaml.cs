using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CreditApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void regBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new Registration();
            window.Show();
            this.Close();
        }

        private void authBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTxtBox.Text.Trim();
            string password = passTxtBox.Password.Trim();
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Заполните поле \"Логин\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                loginTxtBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните поле \"Пароль\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                passTxtBox.Focus();
                return;
            }
            var select = App.Context.Users.FirstOrDefault(u => u.login == login && u.password == password);
            if (select == null)
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            App.CurrentUser = select;
            App.isAdmin = select.id_role == 1;

            if (App.isAdmin)
            {
                new AdminWindow().Show();
            }
            else
            {
                App.CurrentClient = select.Clients.FirstOrDefault();
                new ClientWindow().Show();
            }
            this.Close();
        }
    }
}
