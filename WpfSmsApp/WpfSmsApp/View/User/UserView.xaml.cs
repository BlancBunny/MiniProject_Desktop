using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


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
                rdoAll.IsChecked = true;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView_Page_Loaded : {ex}");
            }
        }

        private async void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new UserAdd());
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView_btn_AddUser : {ex}");
            }
        }
        private async void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new UserEdit());
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView_btn_EditUser : {ex}");
            }
        }
        private async void btnDeactivateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new UserDeactive());
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView_btn_DeactiveUser : {ex}");
            }
        }
        private async void btnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF File(*.pdf)|*.pdf";
            saveDialog.FileName = "";
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    Font font = new Font(Font.FontFamily.HELVETICA, 12);
                    string pdfFilePath = saveDialog.FileName;

                    Document pdfDoc = new Document(PageSize.A4);

                    // PDF 객체생성
                    PdfPTable pdfPTable = new PdfPTable(grdUserData.Columns.Count);

                    // PDF 내용생성
                    Paragraph title = new Paragraph($"PKNU STOCK Management System : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");


                    // PDF 파일생성
                    using (FileStream stream = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(title);
                        pdfDoc.Close();
                        stream.Close();
                        Common.LOGGER.Info($"PDF 파일 생성 : {pdfFilePath}");
                        var metroWindow = Application.Current.MainWindow as MetroWindow;
                        await metroWindow.ShowMessageAsync("파일 생성", $"PDF 파일 생성 완료");
                    }
                }
                catch (Exception ex)
                {
                    Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView_btn_ExportPdfUser : {ex}");
                }
            }

            

        }

        private async void rdoAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Model.User> users = new List<Model.User>();

                if (rdoAll.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers();
                }
                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView : {ex}");
            }
        }
        private async void rdoActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Model.User> users = new List<Model.User>();

                if (rdoActive.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers().Where(u=>u.UserActivated == true).ToList();
                }
                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView : {ex}");
            }
        }
        private async void rdoDeactive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Model.User> users = new List<Model.User>();

                if (rdoDeactive.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers().Where(u => u.UserActivated == false).ToList();
                }
                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView : {ex}");
            }
        }
    }
}
