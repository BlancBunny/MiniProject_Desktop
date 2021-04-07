using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace WpfSmsApp.View.Store
{
    /// <summary>
    /// AccountView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StoreView : Page
    {
        public StoreView()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Model.Store> stores = new List<Model.Store>();
                List<Model.StockStore> stockStores = new List<Model.StockStore>();
                stores = Logic.DataAccess.GetStores();

                foreach (Model.Store item in stores)
                {
                    var store = new Model.StockStore()
                    {
                        StoreID = item.StoreID,
                        StoreName = item.StoreName,
                        StoreLocation = item.StoreLocation,
                        ItemStatus = item.ItemStatus,
                        TagID = item.TagID,
                        BarcodeID = item.BarcodeID,
                        StockQuantity = 0
                    };

                    store.StockQuantity = Logic.DataAccess.GetStocks().Where(t => t.StoreID.Equals(store.StoreID)).Count();
                    stockStores.Add(store);
                }

                this.DataContext = stockStores;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex.Message}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 StoreView_PageLoaded : {ex.Message}");
            }
        }

        private async void btnStoreAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new StoreAdd());
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 UserView_btn_AddUser : {ex}");
            }
        }

        private async void btnStoreEdit_Click(object sender, RoutedEventArgs e)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            if (grdStoreData.SelectedItem == null)
            {
                await metroWindow.ShowMessageAsync("창고 수정", "수정할 창고를 선택하세요.");
                return;
            }
            try
            {
                var storeID = (grdStoreData.SelectedItem as Model.Store).StoreID;
                NavigationService.Navigate(new StoreEdit(storeID));
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                await metroWindow.ShowMessageAsync("예외", $"예외 발생 StoreView_btnStoreEdit_Click : {ex}");
            }
        }


        private async void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            dialog.FileName = "";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    IWorkbook workbook = new XSSFWorkbook();            // 엑셀 파일 생성 
                    ISheet sheet = workbook.CreateSheet("StoreSheet");  // 엑셀 시트 생성 

                    // 첫번째열 생성 
                    IRow rowHeader = sheet.CreateRow(0);
                    ICell cell = rowHeader.CreateCell(0);
                    cell.SetCellValue("순번");
                    cell = rowHeader.CreateCell(1);
                    cell.SetCellValue("창고명");
                    cell = rowHeader.CreateCell(2);
                    cell.SetCellValue("창고위치");
                    cell = rowHeader.CreateCell(3);
                    cell.SetCellValue("재고수");

                    for (int i = 0; i < grdStoreData.Items.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i + 1); 
                        if (grdStoreData.Items[i] is Model.StockStore)
                        {
                            var stockStore = grdStoreData.Items[i] as Model.StockStore;
                            ICell dataCell = row.CreateCell(0);
                            dataCell.SetCellValue(stockStore.StoreID);
                            dataCell = row.CreateCell(1);
                            dataCell.SetCellValue(stockStore.StoreName);
                            dataCell = row.CreateCell(2);
                            dataCell.SetCellValue(stockStore.StoreLocation);
                            dataCell = row.CreateCell(3);
                            dataCell.SetCellValue(stockStore.StockQuantity);
                        }
                    }

                    using (var fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        workbook.Write(fs);
                    }
                    await metroWindow.ShowMessageAsync("엑셀 파일 저장", "Excel File Export Success!");
                }
                catch (Exception ex)
                {
                    Common.LOGGER.Error($"예외 발생 AccountView : {ex}");
                    await metroWindow.ShowMessageAsync("예외", $"예외 발생 StoreView_btnExportExcel_Click : {ex.Message}");
                }
            }

            
        }
        


    }
}
