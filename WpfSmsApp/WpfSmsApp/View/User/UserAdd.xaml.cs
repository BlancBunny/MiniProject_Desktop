using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfSmsApp.View.User
{

    public partial class UserAdd : Page
    {
        public UserAdd()
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
                txtUserIdentityNumber.Focus();
                
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 Account View Loaded : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 AccountView : {ex}");
            }
        }

        


        

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lblUserID.Visibility = lblUserIdentityNumber.Visibility =
                    lblUserSurName.Visibility = lblUserName.Visibility =
                    lblUserEmail.Visibility = lblUserPassword.Visibility =
                    lblUserAdmin.Visibility = lblUserActivated.Visibility = Visibility.Hidden;

            bool isValid = IsValidInput();
            
            if (isValid)
            {
                var user = new Model.User();
                // MessageBox.Show("DB 입력 처리");
              
                user.UserIdentityNumber = txtUserIdentityNumber.Text;
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
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    if (result == 0)
                    {
                        await metroWindow.ShowMessageAsync("입력 실패", "사용자 입력에 문제가 발생했습니다. \n관리자에게 문의하세요.",
                            MessageDialogStyle.Affirmative, null);
                    }
                    else
                    {
                        await metroWindow.ShowMessageAsync("사용자 입력 완료", "",
                            MessageDialogStyle.Affirmative, null);
                        NavigationService.Navigate(new UserView());
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

        public bool IsValidInput()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtUserIdentityNumber.Text))
            {
                lblUserIdentityNumber.Visibility = Visibility.Visible;
                lblUserIdentityNumber.Text = "필수 정보입니다.";
                isValid = false;
            }
            else
            {
                var cnt = Logic.DataAccess.GetUsers().Where(u => u.UserIdentityNumber.Equals(txtUserIdentityNumber.Text)).Count();
                if (cnt > 0)
                {
                    lblUserIdentityNumber.Visibility = Visibility.Visible;
                    lblUserIdentityNumber.Text = "중복된 사번이 존재합니다.";
                    isValid = false;
                }
            }

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
            else
            {
                var cnt = Logic.DataAccess.GetUsers().Where(u => u.UserEmail.Equals(txtUserEmail.Text)).Count();
                if (cnt > 0)
                {
                    lblUserEmail.Visibility = Visibility.Visible;
                    lblUserEmail.Text = "중복된 이메일이 존재합니다.";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(txtUserPassword.Password))
            {
                lblUserPassword.Visibility = Visibility.Visible;
                lblUserPassword.Text = "필수 정보입니다.";
                isValid = false;
            }
            return isValid;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
