using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

namespace WpfSmsApp.View.Account
{
    /// <summary>
    /// AccountView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AccountView : Page
    {
        public AccountView()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = Common.LOGINED_USER;
                txtUserID.Text = user.UserID.ToString();
                txtUserIdentityNumber.Text = user.UserIdentityNumber;
                txtUserSurName.Text = user.UserSurname;
                txtUserName.Text = user.UserName;
                txtUserEmail.Text = user.UserEmail;
                txtUserAdmin.Text = user.UserAdmin.ToString();
                txtUserActivated.Text = user.UserActivated.ToString();
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 Account View Loaded : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 AccountView : {ex}");
            }
        }

        private void btnEditAccount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
