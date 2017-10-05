using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CreatorsNews.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticlesPage : Page
    {
        static int? _articleIndex;
        public ArticlesPage()
        {
            this.InitializeComponent();
            ArticlesGrid.Loaded += this.ArticlesGrid_Loaded;
        }


        private void ArticleClicked(object sender, ItemClickEventArgs e)
        {
            _articleIndex = Array.IndexOf(ViewModel.Articles, e.ClickedItem);


            var container = ArticlesGrid.ContainerFromItem(e.ClickedItem) as GridViewItem;
            if (container != null)
            {
                var root = (FrameworkElement)container.ContentTemplateRoot;
                var image = (UIElement)root.FindName("Image");

                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", image);
            }

            // Add a fade out effect
            Transitions = new TransitionCollection();
            Transitions.Add(new ContentThemeTransition());

            Frame.Navigate(typeof(Views.ArticlePage), _articleIndex);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

            // Don't use vertical entrance animation with connected animation
            if (e.NavigationMode == NavigationMode.Back)
            {
                EntranceTransition.FromVerticalOffset = 0;
            }

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

                    ArticlesGrid.ScrollIntoView(item, ScrollIntoViewAlignment.Default);
                    ArticlesGrid.UpdateLayout();

                    // wait for the scroll into view to complete
                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    var container = ArticlesGrid.ContainerFromItem(item) as GridViewItem;
                    if (container != null)
                    {
                        var root = (FrameworkElement)container.ContentTemplateRoot;
                        var image = (Image)root.FindName("Image");

                        //// Wait for image opened. In future Insider Preview releases, this won't be necessary.
                        //image.Opacity = 0;
                        //image.ImageOpened += (sender_, e_) =>
                        //{
                        //    image.Opacity = 1;
                        //};

                        animation.TryStart(image);

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
