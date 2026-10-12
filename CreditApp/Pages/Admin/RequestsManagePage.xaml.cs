using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CreditApp.Entities;

namespace CreditApp.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для RequestsManagePage.xaml
    /// </summary>
    public partial class RequestsManagePage : Page
    {
        public RequestsManagePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }

        private void LoadRequests()
        {
            requestsGrid.ItemsSource = App.Context.Requests
                .OrderByDescending(r => r.create_date)
                .ToList();
        }

        private void approveBtn_Click(object sender, RoutedEventArgs e)
        {
            var request = GetSelectedWaitingRequest();
            if (request == null)
            {
                return;
            }

            try
            {
                var account = new Accounts
                {
                    id_client = request.id_client,
                    account_number = GenerateAccountNumber(),
                    balance = request.summ_credit,
                    id_credit_plan = GetPlanId(request.perc_credit),
                    month_quantity = request.month_quantity,
                    isActive = true,
                    open_date = DateTime.Now
                };

                App.Context.Accounts.Add(account);
                request.status_req = "Approved";
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось одобрить заявку.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Заявка одобрена, счет клиента открыт!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadRequests();
        }

        private void rejectBtn_Click(object sender, RoutedEventArgs e)
        {
            var request = GetSelectedWaitingRequest();
            if (request == null)
            {
                return;
            }

            try
            {
                request.status_req = "Canceled";
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось отклонить заявку.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadRequests();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }

        private Requests GetSelectedWaitingRequest()
        {
            var request = requestsGrid.SelectedItem as Requests;

            if (request == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            if (request.status_req != "Waiting")
            {
                MessageBox.Show("Заявка уже обработана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            return request;
        }

        private int? GetPlanId(decimal percent)
        {
            var plan = App.Context.CreditPlans.FirstOrDefault(p => p.cred_percent == percent);
            return plan != null ? plan.id_credit_plan : (int?)null;
        }

        private string GenerateAccountNumber()
        {
            var random = new Random();
            string number;

            do
            {
                number = "";
                for (int i = 0; i < 16; i++)
                {
                    number += random.Next(0, 10).ToString();
                }
            }
            while (App.Context.Accounts.Any(a => a.account_number == number));

            return number;
        }
    }
}
