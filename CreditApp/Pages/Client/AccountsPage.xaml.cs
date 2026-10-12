using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CreditApp.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для AccountsPage.xaml
    /// </summary>
    public partial class AccountsPage : Page
    {
        public AccountsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.CurrentClient == null)
            {
                return;
            }

            int clientId = App.CurrentClient.id_client;

            accountsGrid.ItemsSource = App.Context.Accounts
                .Where(a => a.id_client == clientId)
                .Select(a => new
                {
                    a.account_number,
                    a.balance,
                    percent = (decimal?)a.CreditPlans.cred_percent,
                    a.month_quantity,
                    a.isActive,
                    a.open_date
                })
                .ToList();
        }
    }
}
