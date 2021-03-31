using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace WpfSmsApp.View
{
    /// <summary>
    /// LoginView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();
            Common.LOGGER.Info("LoginView Initialized");
        }
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            lblResult.Visibility = Visibility.Hidden; // 결과 레이블 숨김

            if (string.IsNullOrEmpty(txtUserEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                lblResult.Visibility = Visibility.Visible;
                lblResult.Content = "아이디/패스워드를 입력하세요.";
                Common.LOGGER.Warn("ID/PW 미입력 접속시도");
                return;
            }

            try
            {
                var email = txtUserEmail.Text;
                var password = txtPassword.Password;

                var mdHash = MD5.Create();
                password = Common.GetMd5Hash(mdHash, password);

                var isOurUser = Logic.DataAccess.GetUsers()
                    .Where(u => u.UserEmail.Equals(email)
                    && u.UserPassword.Equals(password)
                    && u.UserActivated == true).Count();

                if (isOurUser == 0)
                {
                    lblResult.Visibility = Visibility.Visible;
                    lblResult.Content = "아이디/패스워드가 일치하지 않습니다.";
                    Common.LOGGER.Warn("ID/PW 불일치");
                    return;
                }
                else
                {
                    Common.LOGINED_USER = Logic.DataAccess.GetUsers().Where(u => u.UserEmail.Equals(email)).FirstOrDefault();
                    Common.LOGGER.Info($"{email} 접속");
                    this.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                // 예외처리
                Common.LOGGER.Error($"예외 발생 : {ex}");
                await this.ShowMessageAsync("예외", $"예외 발생 LoginView : {ex}");
            }
            // this.Close();
        }
        private async void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowMessageAsync("종료", "종료하시겠습니까?", 
                MessageDialogStyle.AffirmativeAndNegative, null);

            if (result == MessageDialogResult.Affirmative)
            {
                Common.LOGGER.Info("LogOut");
                Application.Current.Shutdown(0); // 프로그램 종료
            }
                
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserEmail.Focus();
            lblResult.Visibility = Visibility.Hidden;
        }
        private void txtUserEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtPassword.Focus();
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnLogin_Click(sender, e);
        }
    }
}
