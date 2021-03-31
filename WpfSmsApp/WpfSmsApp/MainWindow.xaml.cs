using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using WpfSmsApp.View;
using WpfSmsApp.View.Account;
using WpfSmsApp.View.User;

namespace WpfSmsApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            LoginViewOpen();
        }

        private async void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // TODO : 모든 화면을 해제하고 첫화면으로. 

            var result = await this.ShowMessageAsync("LogOut", "로그아웃 하시겠습니까?",
                MessageDialogStyle.AffirmativeAndNegative, null);

            if (result == MessageDialogResult.Affirmative)
            {
                Common.LOGINED_USER = null;
                btnLoginedId.Content = "";
                LoginViewOpen();
            }
                
        }
        
        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            if (Common.LOGINED_USER != null)
                btnLoginedId.Content = $"{Common.LOGINED_USER.UserEmail} ({Common.LOGINED_USER.UserName})";
        }



        private void LoginViewOpen()
        {
            LoginView loginView = new LoginView();
            loginView.Owner = this;
            loginView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginView.ShowDialog();
        }

        private async void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                activeControl.Content = new AccountView();
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 : {ex}");
                await this.ShowMessageAsync("예외 발생", $"btnAccount_Click 예외 발생",
                    MessageDialogStyle.Affirmative, null);
            }
        }

        private async void btnUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                activeControl.Content = new UserView();
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 : {ex}");
                await this.ShowMessageAsync("예외 발생", $"btnUser_Click 예외 발생", 
                    MessageDialogStyle.Affirmative, null);
            }
        }
    }
}
