using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CreditApp.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        public RequestsPage()
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

            requestsGrid.ItemsSource = App.Context.Requests
                .Where(r => r.id_client == clientId)
                .OrderByDescending(r => r.create_date)
                .ToList();
        }
    }
}
