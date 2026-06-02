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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MilkBase_9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeInterface();
            UpdateDateTime();

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => UpdateDateTime();
            timer.Start();
        }

        private void InitializeInterface()
        {
            // Отображаем информацию о пользователе
            txtUserInfo.Text = $"Пользователь: {LoginWindow.CurrentUserLogin} | Роль: {LoginWindow.CurrentUserRole}";
            txtWelcome.Text = $"Добро пожаловать, {LoginWindow.CurrentUserName}!";

            // Показываем кнопку управления пользователями только для администратора
            if (LoginWindow.CurrentUserRole == "Админ")
            {
                btnUserManagement.Visibility = Visibility.Visible;
            }
            else
            {
                btnUserManagement.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateDateTime()
        {
            txtDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        // Кнопка открытия окна управления пользователями
        private void BtnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new UserManagementWindow();
            userWindow.Owner = this;
            userWindow.ShowDialog();
        }
    }
}

