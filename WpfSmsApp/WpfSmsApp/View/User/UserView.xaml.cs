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

namespace WpfSmsApp.View.User
{
    /// <summary>
    /// AccountView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserView : Page
    {
        public UserView()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView : {ex}");
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnDeactivateUser_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnExportPdf_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
