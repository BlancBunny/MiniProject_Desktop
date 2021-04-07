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
    public partial class StoreEdit : Page
    {
        private bool isValid = true;
        private int StoreID { get; set; }
        private Model.Store CurrentStore { get; set; }

        public StoreEdit()
        {
            InitializeComponent();
        }

        public StoreEdit(int storeID) : this()
        {
            StoreID = storeID;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblStoreName.Visibility = lblStoreLocation.Visibility = Visibility.Hidden;
            txtStoreName.Text = txtStoreLocation.Text = "";
            txtStoreName.Focus();

            try
            {
                CurrentStore = Logic.DataAccess.GetStores().Where(s => s.StoreID.Equals(StoreID)).FirstOrDefault();
                txtStoreID.Text = CurrentStore.StoreID.ToString();
                txtStoreName.Text = CurrentStore.StoreName.ToString();
                txtStoreLocation.Text = CurrentStore.StoreLocation.ToString();
                // store 테이블로부터 내용을 읽어옴.
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 StoreEdit_Page_Loaded : 예외 발생 {ex}");
            }
        }
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            lblStoreName.Visibility = lblStoreLocation.Visibility = Visibility.Hidden;
            isValid = IsValidInput();

            if (isValid)
            {
                // MessageBox.Show("DB 입력 처리");

                CurrentStore.StoreName = txtStoreName.Text;
                CurrentStore.StoreLocation = txtStoreLocation.Text;
                
                try
                {
                    var result = Logic.DataAccess.SetScore(CurrentStore);
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    if (result == 0)
                    {
                        Common.LOGGER.Error("오류", "StoreAdd_btnAdd_Click 오류 발생");
                        await metroWindow.ShowMessageAsync("수정 실패", "창고 수정에 문제가 발생했습니다. \n관리자에게 문의하세요.",
                            MessageDialogStyle.Affirmative, null);
                    }
                    else
                    {
                        await metroWindow.ShowMessageAsync("창고 수정 완료", "",
                            MessageDialogStyle.Affirmative, null);
                        NavigationService.Navigate(new StoreView());
                    }
                }
                catch (Exception ex)
                {
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    await metroWindow.ShowMessageAsync("예외", $"예외 발생 StoreEdit_btnEdit_Click : {ex.Message}");
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
