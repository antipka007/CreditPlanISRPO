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
            ChangeStatus("Approved");
        }

        private void rejectBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatus("Canceled");
        }

        private void ChangeStatus(string status)
        {
            var request = requestsGrid.SelectedItem as Requests;
            if (request == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (request.status_req != "Waiting")
            {
                MessageBox.Show("Заявка уже обработана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                request.status_req = status;
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось обновить заявку.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadRequests();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }
    }
}
