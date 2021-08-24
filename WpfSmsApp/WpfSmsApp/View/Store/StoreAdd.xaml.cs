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
        private bool isValid = true;
        public StoreAdd()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
                lblStoreName.Visibility = lblStoreLocation.Visibility =
                    Visibility.Hidden;

                txtStoreName.Focus();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lblStoreName.Visibility = lblStoreLocation.Visibility = Visibility.Hidden;
            isValid = IsValidInput();
            
            
            if (isValid)
            {
                var store = new Model.Store();
                // MessageBox.Show("DB 입력 처리");

                store.StoreName = txtStoreName.Text;
                store.StoreLocation = txtStoreLocation.Text;
                
                try
                {
                    var result = Logic.DataAccess.SetScore(store);
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    if (result == 0)
                    {
                        Common.LOGGER.Error("오류", "StoreAdd_btnAdd_Click 오류 발생");
                        await metroWindow.ShowMessageAsync("입력 실패", "창고 입력에 문제가 발생했습니다. \n관리자에게 문의하세요.",
                            MessageDialogStyle.Affirmative, null);
                    }
                    else
                    {
                        await metroWindow.ShowMessageAsync("창고 입력 완료", "",
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


        private void txtStoreName_LostFocus(object sender, RoutedEventArgs e)
        {
            isValid = IsValidInput();
        }

        private void txtStoreLocation_LostFocus(object sender, RoutedEventArgs e)
        {
            isValid = IsValidInput();
        }



        public bool IsValidInput()
        {
            isValid = true;
            lblStoreName.Text = lblStoreLocation.Text = "";
            if (string.IsNullOrEmpty(txtStoreName.Text))
            {
                lblStoreName.Visibility = Visibility.Visible;
                lblStoreName.Text = "필수 정보입니다.";
                isValid = false;
            }
            else
            {
                var cnt = Logic.DataAccess.GetStores().Where(u => u.StoreName.Equals(txtStoreName.Text)).Count();
                if (cnt > 0)
                {
                    lblStoreName.Visibility = Visibility.Visible;
                    lblStoreName.Text = "중복된 창고명이 존재합니다.";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(txtStoreLocation.Text))
            {
                lblStoreLocation.Visibility = Visibility.Visible;
                lblStoreLocation.Text = "필수 정보입니다.";
                isValid = false;
            }
            return isValid;
        }
    }
}
