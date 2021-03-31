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

    public partial class UserDeactive : Page
    {
        public UserDeactive()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
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
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void grdUserData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var user = grdUserData.SelectedItem as Model.User;
        }
      

        private async void btnDeactive_Click(object sender, RoutedEventArgs e)
        {
            var user = grdUserData.SelectedItem as Model.User;
            user.UserActivated = false;

            var result = Logic.DataAccess.SetUser(user);
            if (result == 0)
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("오류", $"사용자 수정에 실패했습니다.");
            }
            else NavigationService.Navigate(new UserView());

            if (grdUserData.SelectedItem == null)
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("오류", $"비활성화할 사용자를 선택하세요.");
                return;
            }
        }
    }
}
