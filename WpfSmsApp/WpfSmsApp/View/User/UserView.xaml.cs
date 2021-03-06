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
                    string nanumPath = Path.Combine(Environment.CurrentDirectory,
                        @"NanumGothicCoding.ttf");
                    BaseFont nanumBase = BaseFont.CreateFont(nanumPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var nanumTitle = new Font(nanumBase, 20f); // 타이틀용 나눔 폰트
                    var nanumContents = new Font(nanumBase, 12f); // 본문용 나눔 폰트 

                    // Font font = new Font(Font.FontFamily.HELVETICA, 12);
                    string pdfFilePath = saveDialog.FileName;



                    // PDF 객체생성
                    Document pdfDoc = new Document(PageSize.A4);
                    

                    // PDF 내용생성
                    Paragraph title = new Paragraph($"부경대 재고관리 시스템(SMS)\n", nanumTitle);
                    Paragraph subTitle = new Paragraph($"사용자 리스트 exported : {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n", nanumContents);

                    PdfPTable pdfPTable = new PdfPTable(grdUserData.Columns.Count);
                    pdfPTable.WidthPercentage = 100;

                    // 헤더 
                    foreach (DataGridColumn column in grdUserData.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString(), nanumContents));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfPTable.AddCell(cell);
                    }
                    
                    float[] widths = new float[] { 25f, 50f, 75f, 150f, 50f, 50f };
                    pdfPTable.SetWidths(widths);

                    // 그리드 
                    foreach (var item in grdUserData.Items)
                    {
                        if (item is Model.User)
                        {
                            var temp = item as Model.User;
                            PdfPCell cell = new PdfPCell(new Phrase(temp.UserID.ToString(), nanumContents));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfPTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserIdentityNumber.ToString(), nanumContents));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfPTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserSurname.ToString() + " " + temp.UserName.ToString(), nanumContents));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfPTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserEmail.ToString(), nanumContents));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfPTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserAdmin.ToString(), nanumContents));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfPTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserActivated.ToString(), nanumContents));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfPTable.AddCell(cell);
                        }
                    }

                    // PDF 파일생성
                    using (FileStream stream = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(title);
                        pdfDoc.Add(subTitle);
                        pdfDoc.Add(pdfPTable);
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
