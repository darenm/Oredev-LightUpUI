using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using CreatorsNews.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CreatorsNews.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsPage : Page
    {
        private static int? _articleIndex;

        public NewsPage()
        {
            InitializeComponent();
            ArticlesGrid.Loaded += ArticlesGrid_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;

            // Don't use vertical entrance animation with connected animation
            if (e.NavigationMode == NavigationMode.Back)
            {
                EntranceTransition.FromVerticalOffset = 0;
            }
        }


        private void ArticleClicked(object sender, ItemClickEventArgs e)
        {
            _articleIndex = Array.IndexOf(ViewModel.Articles, e.ClickedItem as Article);
            ArticlesGrid.PrepareConnectedAnimation("Image", e.ClickedItem, "Image");

            // Add a fade out effect
            Transitions = new TransitionCollection { new ContentThemeTransition() };

            Frame.Navigate(typeof(NewsArticlePage), _articleIndex);
        }

        private async void ArticlesGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (_articleIndex != null)
            {
                // May be able to perform backwards Connected Animation
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Image");
                if (animation != null)
                {
                    var item = ViewModel.Articles[_articleIndex.Value];
                    if (item != null)
                    {
                        ArticlesGrid.ScrollIntoView(item, ScrollIntoViewAlignment.Default);
                        // wait for scroll
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                        await ArticlesGrid.TryStartConnectedAnimationAsync(animation, item, "Image");
                    }
                    else
                    {
                        animation.Cancel();
                    }
                }

                _articleIndex = null;
            }
        }
    }
}