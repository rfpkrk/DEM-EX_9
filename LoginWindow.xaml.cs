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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string _captchaCode;
        private readonly Random _random = new Random();
        private readonly (string Login, string Password, string Name, string Role)[] _users = new[]
        {
            ("admin", "admin123", "Администратор", "Админ"),
            ("user", "user123", "Пользователь", "Пользователь"),
            
        };
        public static string CurrentUserRole { get; private set; }
        public static string CurrentUserName { get; private set; }
        public static string CurrentUserLogin { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            GenerateCaptcha();
            txtUsername.Focus();
        }

        private void GenerateCaptcha()
        {
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";
            int length = _random.Next(5, 7);

            _captchaCode = "";
            for (int i = 0; i < length; i++)
            {
                _captchaCode += chars[_random.Next(chars.Length)];
            }
            txtCaptcha.Text = string.Join(" ", _captchaCode.ToCharArray());
            txtCaptchaInput.Text = "";
        }

        private void BtnRefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            string captchaInput = txtCaptchaInput.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(captchaInput))
            {
                MessageBox.Show("Введите код подтверждения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (captchaInput != _captchaCode)
            {
                MessageBox.Show("Неверный код подтверждения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                GenerateCaptcha();
                txtCaptchaInput.Text = "";
                return;
            }

            foreach (var user in _users)
            {
                if (user.Login == username && user.Password == password)
                {
                    CurrentUserLogin = user.Login;
                    CurrentUserName = user.Name;
                    CurrentUserRole = user.Role;

                    MessageBox.Show($"Вы успешно авторизировались, {user.Name}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Hide();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.ShowDialog();
                    this.Close();
                    return;
                }
            }

            MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            txtPassword.Password = "";
            GenerateCaptcha();
            txtCaptchaInput.Text = "";
            txtPassword.Focus();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateLoginButton()
        {
            btnLogin.IsEnabled = !string.IsNullOrEmpty(txtUsername.Text) &&
                                !string.IsNullOrEmpty(txtPassword.Password) &&
                                !string.IsNullOrEmpty(txtCaptchaInput.Text);
        }

        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLoginButton();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateLoginButton();
        }

        private void TxtCaptchaInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLoginButton();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && btnLogin.IsEnabled)
            {
                BtnLogin_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                BtnCancel_Click(sender, e);
            }
        }
    }
}

