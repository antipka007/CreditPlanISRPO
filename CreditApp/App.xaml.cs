using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CreditApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.BankEntities Context { get; } = new Entities.BankEntities();
        public static Entities.Users CurrentUser { get; set; }
        public static bool isAdmin { get; set; }
    }
}
