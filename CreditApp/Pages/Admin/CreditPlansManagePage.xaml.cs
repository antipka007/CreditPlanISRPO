using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CreditApp.Entities;

namespace CreditApp.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для CreditPlansManagePage.xaml
    /// </summary>
    public partial class CreditPlansManagePage : Page
    {
        private CreditPlans _selectedPlan;

        public CreditPlansManagePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlans();
        }

        private void LoadPlans()
        {
            plansGrid.ItemsSource = App.Context.CreditPlans.ToList();
        }

        private void plansGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPlan = plansGrid.SelectedItem as CreditPlans;

            if (_selectedPlan != null)
            {
                percentBox.Text = _selectedPlan.cred_percent.ToString();
                maxBox.Text = _selectedPlan.max_balance.ToString();
            }
        }

        private bool TryReadInput(out decimal percent, out decimal maxBalance)
        {
            percent = 0;
            maxBalance = 0;

            if (!decimal.TryParse(percentBox.Text.Trim(), out percent) || percent < 0)
            {
                MessageBox.Show("Введите корректный процент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                percentBox.Focus();
                return false;
            }

            if (!decimal.TryParse(maxBox.Text.Trim(), out maxBalance) || maxBalance <= 0)
            {
                MessageBox.Show("Введите корректную максимальную сумму", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                maxBox.Focus();
                return false;
            }

            return true;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            decimal percent, maxBalance;
            if (!TryReadInput(out percent, out maxBalance))
            {
                return;
            }

            try
            {
                App.Context.CreditPlans.Add(new CreditPlans
                {
                    cred_percent = percent,
                    max_balance = maxBalance
                });
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить план.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ClearForm();
            LoadPlans();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPlan == null)
            {
                MessageBox.Show("Выберите план для изменения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal percent, maxBalance;
            if (!TryReadInput(out percent, out maxBalance))
            {
                return;
            }

            try
            {
                _selectedPlan.cred_percent = percent;
                _selectedPlan.max_balance = maxBalance;
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось изменить план.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ClearForm();
            LoadPlans();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPlan == null)
            {
                MessageBox.Show("Выберите план для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var confirm = MessageBox.Show("Удалить выбранный план?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                App.Context.CreditPlans.Remove(_selectedPlan);
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось удалить план.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ClearForm();
            LoadPlans();
        }

        private void ClearForm()
        {
            _selectedPlan = null;
            percentBox.Clear();
            maxBox.Clear();
            plansGrid.SelectedItem = null;
        }
    }
}
