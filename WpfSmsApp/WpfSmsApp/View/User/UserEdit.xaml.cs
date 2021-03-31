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

    public partial class UserEdit : Page
    {
        public UserEdit()
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

                // DataGrid
                List<Model.User> users = new List<Model.User>();
                users = Logic.DataAccess.GetUsers();
                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 Account View Loaded : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 AccountView : {ex}");
            }
        }
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            lblUserID.Visibility = lblUserIdentityNumber.Visibility =
                    lblUserSurName.Visibility = lblUserName.Visibility =
                    lblUserEmail.Visibility = lblUserPassword.Visibility =
                    lblUserAdmin.Visibility = lblUserActivated.Visibility = Visibility.Hidden;

            bool isValid = IsValidInput();
            
            if (isValid)
            {
                var user = grdUserData.SelectedItem as Model.User;
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
                        await metroWindow.ShowMessageAsync("사용자 정보 수정 완료", "",
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

        private void grdUserData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var user = grdUserData.SelectedItem as Model.User;
            
            txtUserID.Text = user.UserID.ToString();
            txtUserIdentityNumber.Text = user.UserIdentityNumber;
            txtUserSurName.Text = user.UserSurname;
            txtUserName.Text = user.UserName;
            txtUserEmail.Text = user.UserEmail;
            cboUserAdmin.SelectedIndex = user.UserAdmin == true ? 1 : 0;
            cboUserActivated.SelectedIndex = user.UserActivated == true ? 1 : 0;


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

            if (cboUserAdmin.SelectedIndex < 0)
            {
                lblUserAdmin.Visibility = Visibility.Visible;
                lblUserAdmin.Text = "필수 정보입니다.";
                isValid = false;
            }

            if (cboUserActivated.SelectedIndex < 0)
            {
                lblUserActivated.Visibility = Visibility.Visible;
                lblUserActivated.Text = "필수 정보입니다.";
                isValid = false;
            }
            return isValid;
        }

        
    }
}
