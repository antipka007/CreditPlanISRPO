using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CreditApp.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var rows = new List<UserRow>();

            foreach (var user in App.Context.Users.Where(u => u.Roles.name_role == "Клиент").ToList())
            {
                var client = user.Clients.FirstOrDefault();

                rows.Add(new UserRow
                {
                    login = user.login,
                    role = user.Roles != null ? user.Roles.name_role : "",
                    fio = client == null
                        ? ""
                        : (client.secondname + " " + client.firstname + " " + (client.fathersname ?? "")).Trim(),
                    email = client == null ? "" : client.email,
                    phone = client == null ? "" : client.phone,
                    banned = client != null && client.isBanned
                });
            }

            usersGrid.ItemsSource = rows;
        }

        public class UserRow
        {
            public string login { get; set; }
            public string role { get; set; }
            public string fio { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public bool banned { get; set; }
        }
    }
}
