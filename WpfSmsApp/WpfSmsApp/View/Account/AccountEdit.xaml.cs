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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfSmsApp.Model;

namespace WpfSmsApp.View.Account
{
    /// <summary>
    /// AccountView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AccountEdit : Page
    {
        public AccountEdit()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblUserID.Visibility = lblUserIdentityNumber.Visibility = 
                    lblUserSurName.Visibility = lblUserName.Visibility =
                    lblUserEmail.Visibility = lblUserPassword.Visibility = 
                    lblUserAdmin.Visibility = lblUserActivated.Visibility = Visibility.Hidden;

                // ComboBox Initialized
                List<string> comboValue = new List<string>
                {
                    "False",
                    "True"
                };

                cboUserAdmin.ItemsSource = cboUserActivated.ItemsSource = comboValue;

                var user = Common.LOGINED_USER;
                txtUserID.Text = user.UserID.ToString();
                txtUserIdentityNumber.Text = user.UserIdentityNumber;
                txtUserSurName.Text = user.UserSurname;
                txtUserName.Text = user.UserName;
                txtUserEmail.Text = user.UserEmail;
                // txtUserPassword.Password = user.UserPassword;
                cboUserAdmin.SelectedIndex = user.UserAdmin == true ? 1 : 0;
                cboUserActivated.SelectedIndex = user.UserActivated == true ? 1 : 0;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 Account View Loaded : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 AccountView : {ex}");
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; // Valid Check 
            lblUserID.Visibility = lblUserIdentityNumber.Visibility =
                    lblUserSurName.Visibility = lblUserName.Visibility =
                    lblUserEmail.Visibility = lblUserPassword.Visibility =
                    lblUserAdmin.Visibility = lblUserActivated.Visibility = Visibility.Hidden;

            if (string.IsNullOrEmpty(txtUserSurName.Text))
            {
                lblUserSurName.Visibility = Visibility.Visible;
                lblUserSurName.Text = "필수 정보입니다.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                lblUserName.Visibility = Visibility.Visible;
                lblUserName.Text = "필수 정보입니다.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtUserEmail.Text))
            {
                lblUserEmail.Visibility = Visibility.Visible;
                lblUserEmail.Text = "필수 정보입니다.";
                isValid = false;
            }
            
            if (string.IsNullOrEmpty(txtUserPassword.Password))
            {
                lblUserPassword.Visibility = Visibility.Visible;
                lblUserPassword.Text = "필수 정보입니다.";
                isValid = false;
            }

            if (isValid)
            {
                // MessageBox.Show("DB 처리");
                var user = Common.LOGINED_USER;
                user.UserSurname = txtUserSurName.Text;
                user.UserName = txtUserName.Text;
                user.UserEmail = txtUserEmail.Text;
                user.UserPassword = txtUserPassword.Password;
                user.UserAdmin = bool.Parse(cboUserAdmin.SelectedValue.ToString());
                user.UserActivated = bool.Parse(cboUserActivated.SelectedValue.ToString());

                try
                {
                    var mdHash = MD5.Create();
                    user.UserPassword = Common.GetMd5Hash(mdHash, user.UserPassword);

                    var result = Logic.DataAccess.SetUser(user); 
                    if (result == 0)
                    {
                        var metroWindow = Application.Current.MainWindow as MetroWindow;
                        await metroWindow.ShowMessageAsync("수정 실패", "계정 수정에 문제가 발생했습니다. \n관리자에게 문의하세요.",
                            MessageDialogStyle.Affirmative, null);
                    }
                    else
                    {
                        var metroWindow = Application.Current.MainWindow as MetroWindow;
                        await metroWindow.ShowMessageAsync("수정 완료", "정상적으로 수정했습니다", 
                            MessageDialogStyle.Affirmative, null);
                    }
                }
                catch (Exception ex)
                {
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    await metroWindow.ShowMessageAsync("예외", $"예외 발생 AccountView : {ex}");
                    Common.LOGGER.Error($"예외 발생 AccountEdit : {ex}");
                }
                
            }
        }

        

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        private void ChangeErrorMessage()
        {

        }

        
    }
}
