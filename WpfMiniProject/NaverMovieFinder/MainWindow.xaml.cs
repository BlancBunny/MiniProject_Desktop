using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NaverMovieFinder.helper;
using NaverMovieFinder.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace NaverMovieFinder
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


        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            common.isFavorite = false;
        }
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            stsResult.Content = "";
            imgPoster.Source = new BitmapImage(new Uri("no_picture.jpg", UriKind.RelativeOrAbsolute));

            if (string.IsNullOrEmpty(txtSearchMovie.Text))
            {
                stsResult.Content = "검색할 영화명을 입력 후, 검색 버튼을 눌러주세요.";
                await this.ShowMessageAsync("오류", "검색할 영화명을 입력 후, 검색 버튼을 눌러주세요.", 
                    MessageDialogStyle.Affirmative, null);
                return;
            }

            try
            {
                SearchNaverApi(txtSearchMovie.Text);
                btnAddWatchList.IsEnabled = true;
                btnDeleteWatchList.IsEnabled = false;
            }
            catch (Exception ex)
            {
                common.LOGGER.Error($"예외 발생 : {ex}");
                await this.ShowMessageAsync("오류", $"예외 발생 : {ex.Message}", 
                    MessageDialogStyle.Affirmative, null);
            }
            
        }
        private void txtSearchMovie_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }
        private async void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if (grdData.SelectedItem is MovieItem)
                {
                    var movie = grdData.SelectedItem as MovieItem;
                    // await this.ShowMessageAsync("결과", $"{movie.Image}");
                    if (string.IsNullOrEmpty(movie.Image)) imgPoster.Source =
                            new BitmapImage(new Uri("no_picture.jpg", UriKind.RelativeOrAbsolute));
                    else imgPoster.Source = new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                common.LOGGER.Error($"예외 발생 : {ex}");
                await this.ShowMessageAsync("오류", $"예외 발생 : {ex.Message}",
                    MessageDialogStyle.Affirmative, null);
            }

            try
            {
                if (grdData.SelectedItem is FavoriteMovies)
                {
                    var movie = grdData.SelectedItem as FavoriteMovies;
                    // await this.ShowMessageAsync("결과", $"{movie.Image}");
                    if (string.IsNullOrEmpty(movie.Image)) imgPoster.Source =
                            new BitmapImage(new Uri("no_picture.jpg", UriKind.RelativeOrAbsolute));
                    else imgPoster.Source = new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                common.LOGGER.Error($"예외 발생 : {ex}");
                await this.ShowMessageAsync("오류", $"예외 발생 : {ex.Message}",
                    MessageDialogStyle.Affirmative, null);
            }


        }
        private async void btnAddWatchList_Click(object sender, RoutedEventArgs e)
        {
            if (grdData.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기에 추가할 영화를 선택하세요.", 
                    MessageDialogStyle.Affirmative, null);
                return;
            }

            // grdData.SelectedItem : MovieItem -> FavoriteMovies (Entity Framework) 로 클래스 변경
            List<FavoriteMovies> movies = new List<FavoriteMovies>();
            foreach (MovieItem item in grdData.SelectedItems)
            {
                FavoriteMovies temp = new FavoriteMovies()
                {
                    Title = item.Title,
                    Link = item.Link,
                    Image = item.Image,
                    SubTitle = item.SubTitle,
                    PubDate = item.PubDate,
                    Director = item.Director,
                    Actor = item.Actor,
                    UserRating = item.UserRating,
                    RegDate = DateTime.Now
                };
                movies.Add(temp);
            }

            using (var ctx = new OpenApiLabEntities())
            {
                try
                {
                    ctx.Set<FavoriteMovies>().AddRange(movies);
                    ctx.SaveChanges();
                    stsResult.Content = $"즐겨찾기 {movies.Count}건 추가 완료.";
                    await this.ShowMessageAsync("저장 성공", $"영화 {movies.Count}개를 즐겨찾기에 추가했습니다", 
                        MessageDialogStyle.Affirmative, null);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.HResult == -2146232060)
                        await this.ShowMessageAsync("오류", $"이미 즐겨찾기에 있는 영화가 포함되어 있습니다.",
                                        MessageDialogStyle.Affirmative, null);
                    else
                    {
                        await this.ShowMessageAsync("오류", $"예외 발생 : {ex.Message}",
                                        MessageDialogStyle.Affirmative, null);
                        common.LOGGER.Error($"예외 발생 : {ex}");
                    }
                    
                }
            } 

        }
        private void btnViewWatchList_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            IsFavorite();
        }
        private async void btnDeleteWatchList_Click(object sender, RoutedEventArgs e)
        {
            if (grdData.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("오류", "삭제할 즐겨찾기 영화를 선택하세요");
                return;
            }
            // List<FavoriteMovies> removeList = new List<FavoriteMovies>();
            foreach (FavoriteMovies item in grdData.SelectedItems)
            {
                // 삭제
                using (var ctx = new OpenApiLabEntities())
                {
                    var itemDelete = ctx.FavoriteMovies.Find(item.Idx);
                    ctx.Entry(itemDelete).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }

            btnViewWatchList_Click(sender, e);
        }
        private async void btnNaverMovieLink_Click(object sender, RoutedEventArgs e)
        {
            if (grdData.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("네이버영화", "영화를 선택하세요.");
                return;
            }
            if (grdData.SelectedItems.Count > 1)
            {
                await this.ShowMessageAsync("네이버영화", "영화를 하나만 선택하세요.");
                return;
            }

            string linkUrl = "";
            if (common.isFavorite) // 즐겨찾기 상태
            {
                var item = grdData.SelectedItem as FavoriteMovies;
                linkUrl = item.Link;
            }
            else // 네이버 API 검색 결과
            {
                var item = grdData.SelectedItem as MovieItem;
                linkUrl = item.Link;
            }

            Process.Start(linkUrl);
        }
        private async void btnWatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            if (grdData.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("오류", "영화를 선택하세요.");
                return;
            }
            if (grdData.SelectedItems.Count > 1)
            {
                await this.ShowMessageAsync("오류", "영화를 하나만 선택하세요.");
                return;
            }

            string movieName = "";
            if (common.isFavorite)
            {
                var item = grdData.SelectedItem as FavoriteMovies;
                movieName = item.Title;
            }
            else
            {
                var item = grdData.SelectedItem as MovieItem;
                movieName = item.Title;
            }

            var trailerWindow = new TrailerWindow(movieName);
            trailerWindow.Owner = this;
            trailerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            trailerWindow.ShowDialog();
        }
        private async void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            var result = await this.ShowMessageAsync(this.Title, "종료하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative, null);
            if (result == MessageDialogResult.Negative) return;
            else Application.Current.Shutdown();
        }






#region CustomMethod

        /// <summary>
        /// 즐겨찾기 목록 불러오기 (Entity Framework)
        /// </summary>
        /// <returns></returns>
        private List<FavoriteMovies> FavoriteList()
        {
            // List<MovieItem> movies = new List<MovieItem>();
            List<FavoriteMovies> favorites = new List<FavoriteMovies>();

            using (var ctx = new OpenApiLabEntities())
            {
                favorites = ctx.FavoriteMovies.ToList();
            }
            return favorites;
        }


        /// <summary>
        /// Naver Api 
        /// </summary>
        /// <param name="movieName"></param>
        private void SearchNaverApi(string movieName)
        {
            string clientID = "K3syzZS40w59ePgVVyE1";
            string clientSecret = "9tbm63LFyX";
            string openApiUrl = $"https://openapi.naver.com/v1/search/movie?start=1&display=30&query={movieName}";

            string resJson = common.GetApiResult(openApiUrl, clientID, clientSecret);

            var parsedJson = JObject.Parse(resJson);

            int total = Convert.ToInt32(parsedJson["total"]);
            int display = Convert.ToInt32(parsedJson["display"]);

            stsResult.Content = $"{total}건 중 {display}건 호출 성공";

            var items = parsedJson["items"];
            JArray jArray = (JArray)items;

            List<MovieItem> movieItems = new List<MovieItem>();

            foreach (var item in jArray)
            {
                MovieItem movie = new MovieItem(
                    common.StripHtmlTag(item["title"].ToString()),
                    item["link"].ToString(),
                    item["image"].ToString(),
                    item["subtitle"].ToString(),
                    item["pubDate"].ToString(),
                    common.StripPipe(item["director"].ToString()),
                    common.StripPipe(item["actor"].ToString()),
                    item["userRating"].ToString()
                    );
                movieItems.Add(movie);
            }

            grdData.DataContext = movieItems;
        }


        /// <summary>
        /// 즐겨찾기/검색창 변환 
        /// </summary>
        private void IsFavorite()
        {
            if (common.isFavorite) // 즐겨찾기 상태에서 실행
            {
                imgPoster.Source = new BitmapImage(new Uri("no_picture.jpg", UriKind.RelativeOrAbsolute));
                btnViewWatchList.Content = "즐겨찾기 보기";
                btnDeleteWatchList.IsEnabled = false;
                grdData.DataContext = "";
                txtSearchMovie.Text = "";
                txtSearchMovie.Focus();
                common.isFavorite = false;
            }
            else // 검색창 상태에서 실행
            {
                btnViewWatchList.Content = "영화 검색하기";
                btnAddWatchList.IsEnabled = false;
                btnDeleteWatchList.IsEnabled = true;
                grdData.DataContext = FavoriteList();
                common.isFavorite = true;
            }
        }

#endregion

    }
}
