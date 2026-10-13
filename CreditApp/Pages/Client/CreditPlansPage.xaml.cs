using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CreditApp.Entities;

namespace CreditApp.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для CreditPlansPage.xaml
    /// </summary>
    public partial class CreditPlansPage : Page
    {
        private CreditPlans _selectedPlan;
        private List<CreditPlans> _allPlans = new List<CreditPlans>();

        public CreditPlansPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _allPlans = App.Context.CreditPlans.ToList();
            ApplyFilter();
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void clearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            percentFilterBox.Clear();
            maxFilterBox.Clear();
        }

        private void ApplyFilter()
        {
            decimal percentFilter;
            bool hasPercentFilter = decimal.TryParse(percentFilterBox.Text.Trim(), out percentFilter);

            decimal maxFilter;
            bool hasMaxFilter = decimal.TryParse(maxFilterBox.Text.Trim(), out maxFilter);

            IEnumerable<CreditPlans> filtered = _allPlans;

            if (hasPercentFilter)
            {
                filtered = filtered.Where(p => p.cred_percent <= percentFilter);
            }

            if (hasMaxFilter)
            {
                filtered = filtered.Where(p => p.max_balance >= maxFilter);
            }

            plansGrid.ItemsSource = filtered.ToList();
        }

        private void plansGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPlan = plansGrid.SelectedItem as CreditPlans;

            if (_selectedPlan != null)
            {
                selectedPlanText.Text = string.Format(
                    "Выбран план №{0}: {1}% годовых, максимальная сумма {2:N2}",
                    _selectedPlan.id_credit_plan,
                    _selectedPlan.cred_percent,
                    _selectedPlan.max_balance);
            }
            else
            {
                selectedPlanText.Text = "План не выбран";
            }
        }

        private void sendRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPlan == null)
            {
                MessageBox.Show("Выберите кредитный план", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal sum;
            if (!decimal.TryParse(sumBox.Text.Trim(), out sum) || sum <= 0)
            {
                MessageBox.Show("Введите корректную сумму кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                sumBox.Focus();
                return;
            }

            int months;
            if (!int.TryParse(monthsBox.Text.Trim(), out months) || months <= 0)
            {
                MessageBox.Show("Введите корректный срок в месяцах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                monthsBox.Focus();
                return;
            }

            if (sum > _selectedPlan.max_balance)
            {
                MessageBox.Show(
                    string.Format("Сумма превышает максимально допустимую ({0:N2})", _selectedPlan.max_balance),
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                sumBox.Focus();
                return;
            }

            if (App.CurrentClient == null)
            {
                MessageBox.Show("Профиль клиента не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var request = new Requests
                {
                    id_client = App.CurrentClient.id_client,
                    summ_credit = sum,
                    perc_credit = _selectedPlan.cred_percent,
                    month_quantity = months,
                    status_req = "Waiting",
                    create_date = DateTime.Now
                };

                App.Context.Requests.Add(request);
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось отправить заявку.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Заявка отправлена на рассмотрение!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            sumBox.Clear();
            monthsBox.Clear();
        }
    }
}
