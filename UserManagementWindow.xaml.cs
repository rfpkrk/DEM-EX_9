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
using System.Windows.Shapes;

namespace MilkBase_9
{
    /// <summary>
    /// Логика взаимодействия для UserManagementWindow.xaml
    /// </summary>
    public partial class UserManagementWindow : Window
    {
        private User selectedUser;

        public UserManagementWindow()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            UsersGrid.ItemsSource = null;
            UsersGrid.ItemsSource = UserService.GetAllUsers();
        }
        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UsersGrid.SelectedItem as User;
            bool hasSelection = selectedUser != null;
            btnUpdate.IsEnabled = hasSelection;
            btnUnblock.IsEnabled = hasSelection && selectedUser.IsBlocked;
            btnDelete.IsEnabled = hasSelection && selectedUser.Login != "admin";
            if (selectedUser != null)
            {
                txtLogin.Text = selectedUser.Login;
                txtFullName.Text = selectedUser.FullName;
                cmbRole.SelectedIndex = selectedUser.Role == "Администратор" ? 1 : 0;
                chkIsBlocked.IsChecked = selectedUser.IsBlocked;
                txtPassword.Password = "";
            }
            else
            {
                ClearForm();
            }
        }
        private void ClearForm()
        {
            txtLogin.Text = "";
            txtFullName.Text = "";
            cmbRole.SelectedIndex = 0;
            chkIsBlocked.IsChecked = false;
            txtPassword.Password = "";
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;
            string fullName = txtFullName.Text.Trim();
            string role = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (UserService.AddUser(login, password, fullName, role))
            {
                MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUsers();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null) return;

            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;
            string fullName = txtFullName.Text.Trim();
            string role = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();
            bool isBlocked = chkIsBlocked.IsChecked ?? false;
            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (UserService.UpdateUser(selectedUser.Id, login, password, fullName, role, isBlocked))
            {
                MessageBox.Show("Данные пользователя обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnUnblock_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                UserService.UnblockUser(selectedUser.Id);
                MessageBox.Show("Блокировка снята!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUsers();
            }
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                var result = MessageBox.Show($"Удалить пользователя '{selectedUser.Login}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes && UserService.DeleteUser(selectedUser.Id))
                {
                    MessageBox.Show("Пользователь удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsers();
                    ClearForm();
                }
            }
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
            ClearForm();
        }
    }
}
