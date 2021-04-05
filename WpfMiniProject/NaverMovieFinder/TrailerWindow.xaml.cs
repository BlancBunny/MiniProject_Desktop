using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NaverMovieFinder.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NaverMovieFinder
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {
        public TrailerWindow()
        {
            InitializeComponent();
        }

        public TrailerWindow(string movieName) : this()
        {
            lblMovieName.Content = $"{movieName} 예고편";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 유튜브 API로 검색
            // MessageBox.Show("유튜브 검색");
            ProcSearchYoutubeApi();
        }

        private async void ProcSearchYoutubeApi()
        {
            await LoadDataCollection();
        }

        private async Task LoadDataCollection()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBDO048WJAAPZlGUgAmV16xuhCXcWnA6YI",
                ApplicationName = this.GetType().ToString()
            });

            var request = youtubeService.Search.List("Snippet");
            request.Q = lblMovieName.Content.ToString();
            request.MaxResults = 10;

            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                if (item.Id.Kind.Equals("youtube#video"))
                {
                    YoutubeItem youtube = new YoutubeItem()
                    {
                        Title = item.Snippet.Title,
                        Author = item.Snippet.ChannelTitle,
                        URL = $"https://www.youtube.com/watch?v={item.Id.VideoId}"
                    };
                    
                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));
                    lsvYoutubeSearch.Items.Add(youtube);
                }
            }
        }

        private async void lsvYoutubeSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsvYoutubeSearch.SelectedItems.Count == 0)
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("유튜브보기", $"영화를 선택하세요.");
                return;
            }
            if (lsvYoutubeSearch.SelectedItems.Count > 1)
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                await metroWindow.ShowMessageAsync("유튜브보기", $"영화를 하나만 선택하세요.");
                return;
            }

            if (lsvYoutubeSearch.SelectedItem is YoutubeItem)
            {
                var video = lsvYoutubeSearch.SelectedItem as YoutubeItem;
                brwYoutubeWatch.Source = new Uri(video.URL, UriKind.RelativeOrAbsolute);
                // Process.Start(video.URL);
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            brwYoutubeWatch.Source = null;
            brwYoutubeWatch.Dispose();
        }
    }
}
