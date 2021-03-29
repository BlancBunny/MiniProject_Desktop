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
using WpfSmsApp.View;

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
            var result = await this.ShowMessageAsync("Log Out", "로그아웃 하시겠습니까?",
                MessageDialogStyle.AffirmativeAndNegative, null);

            if (result == MessageDialogResult.Affirmative)
                LoginViewOpen();
        }
        private void LoginViewOpen()
        {
            LoginView loginView = new LoginView();
            loginView.Owner = this;
            loginView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginView.ShowDialog();
        }
    }
}
