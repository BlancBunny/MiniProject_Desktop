using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfSmsApp.View.Store
{

    public partial class StoreAdd : Page
    {
        public StoreAdd()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblUserID.Visibility = lblUserIdentityNumber.Visibility =
                    Visibility.Hidden;

                txtUserIdentityNumber.Focus();
                
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 Account View Loaded : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 StoreAdd_Page_Loaded : {ex}");
            }
        }

        


        

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lblUserID.Visibility = lblUserIdentityNumber.Visibility =
                    lblUserSurName.Visibility = Visibility.Hidden;

            bool isValid = IsValidInput();
            
            if (isValid)
            {
                var user = new Model.User();
                // MessageBox.Show("DB 입력 처리");
              
                user.UserIdentityNumber = txtUserIdentityNumber.Text;
                user.UserSurname = txtUserSurName.Text;
                
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
                        NavigationService.Navigate(new StoreView());
                    }
                }
                catch (Exception ex)
                {
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    await metroWindow.ShowMessageAsync("예외", $"예외 발생 StoreAdd_btnAdd_Click : {ex.Message}");
                    Common.LOGGER.Error($"예외 발생 StoreAdd_btnAdd_Click : {ex}");
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

           
            return isValid;
        }

        
    }
}
