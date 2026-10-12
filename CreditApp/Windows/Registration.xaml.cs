using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CreditApp.Entities;

namespace CreditApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private bool _goToAuthorization = true;

        public Registration()
        {
            InitializeComponent();
        }

        private void regBtn_Click(object sender, RoutedEventArgs e)
        {
            string secondname = secondNameTxtBox.Text.Trim();
            string firstname = firstNameTxtBox.Text.Trim();
            string fathersname = fathersnameNameTxtBox.Text.Trim();
            string email = emailTxtBox.Text.Trim();
            string phone = phoneTxtBox.Text.Trim();
            string login = loginTxtBox.Text.Trim();
            string password = passTxtBox.Password.Trim();
            string doublePassword = doublePassTxtBox.Password.Trim();

            if (string.IsNullOrEmpty(secondname))
            {
                ShowError("Заполните поле \"Фамилия\"", secondNameTxtBox);
                return;
            }
            if (string.IsNullOrEmpty(firstname))
            {
                ShowError("Заполните поле \"Имя\"", firstNameTxtBox);
                return;
            }
            if (string.IsNullOrEmpty(fathersname))
            {
                ShowError("Заполните поле \"Отчество\"", fathersnameNameTxtBox);
                return;
            }
            if (string.IsNullOrEmpty(login))
            {
                ShowError("Заполните поле \"Логин\"", loginTxtBox);
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                ShowError("Заполните поле \"Пароль\"", passTxtBox);
                return;
            }
            if (string.IsNullOrEmpty(doublePassword))
            {
                ShowError("Повторите пароль", doublePassTxtBox);
                return;
            }
            if (password != doublePassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                doublePassTxtBox.Clear();
                doublePassTxtBox.Focus();
                return;
            }
            if (App.Context.Users.Any(u => u.login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                loginTxtBox.Clear();
                loginTxtBox.Focus();
                return;
            }

            var clientRole = App.Context.Roles.FirstOrDefault(r => r.name_role == "Клиент");
            if (clientRole == null)
            {
                MessageBox.Show("Роль \"Клиент\" не найдена в базе данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string gender = ((ComboBoxItem)genderBox.SelectedItem).Content.ToString();

            try
            {
                var user = new Users
                {
                    login = login,
                    password = password,
                    id_role = clientRole.id_role
                };

                var client = new Clients
                {
                    Users = user,
                    secondname = secondname,
                    firstname = firstname,
                    fathersname = fathersname,
                    gender = gender,
                    email = string.IsNullOrEmpty(email) ? null : email,
                    phone = string.IsNullOrEmpty(phone) ? null : phone,
                    isBanned = false
                };

                App.Context.Users.Add(user);
                App.Context.Clients.Add(client);
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось зарегистрировать пользователя.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            _goToAuthorization = false;
            new Authorization().Show();
            this.Close();
        }

        private void ShowError(string message, Control control)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            control.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_goToAuthorization)
            {
                var window = new Authorization();
                window.Show();
            }
        }
    }
}
