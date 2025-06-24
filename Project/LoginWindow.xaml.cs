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

namespace Project
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService;

        public LoginWindow(AuthService authService)
        {
            InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text?.Trim();
            var password = PasswordBox.Password?.Trim();

            // ✅ Kiểm tra tài khoản trống
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorText.Text = "Vui lòng nhập đủ tài khoản và mật khẩu.";
                return;
            }

            try
            {
                var user = await _authService.ValidateUserAsync(username, password);
                if (user == null)
                {
                    ErrorText.Text = "Invalid username or password.";
                    return;
                }

                switch (user.Role.ToLower())
                {
                    case "admin":
                        new AdminMainWindow().Show();
                        this.Close();
                        break;

                    case "student":
                        new StudentMainWindow(user).Show();
                        this.Close();
                        break;

                    default:
                        ErrorText.Text = "User role not recognized.";
                        break;
                }
            }
            catch (Exception ex)
            {
                // ⚡️ Log ex.Message nếu cần
                ErrorText.Text = "Có lỗi trong quá trình đăng nhập. Vui lòng thử lại.";
            }
        }
    }
}
