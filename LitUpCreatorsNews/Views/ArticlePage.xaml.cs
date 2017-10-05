using System;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CreatorsNews.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticlePage : Page
    {
        private static ScalarKeyFrameAnimation OpacityAnimation;
        private Visual _articleVisual;

        public ArticlePage()
        {
            InitializeComponent();
            EnsureAnimation();
            _articleVisual = ElementCompositionPreview.GetElementVisual(ArticlePagePanel);
            _articleVisual.Opacity = 0;
        }

        private void EnsureAnimation()
        {
            if (OpacityAnimation != null)
            {
                return;
            }

            var linear = Window.Current.Compositor.CreateLinearEasingFunction();
            OpacityAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimation.InsertKeyFrame(0, 0);
            OpacityAnimation.InsertKeyFrame(0.2f, 0, linear);
            OpacityAnimation.InsertKeyFrame(1, 1, linear);
            OpacityAnimation.Duration = TimeSpan.FromMilliseconds(600);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var articleIndex = (int) e.Parameter;
            ViewModel.LoadArticle(articleIndex);

            BackgroundImage.Source = ViewModel.Article.MainImage;

            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Image");
            if (animation != null)
            {
                BackgroundImage.Opacity = 0;
                // Wait for image opened. In future Insider Preview releases, this won't be necessary.
                BackgroundImage.ImageOpened += (sender_, e_) =>
                {
                    BackgroundImage.Opacity = 1;
                    animation.TryStart(BackgroundImage);
                    _articleVisual.StartAnimation(nameof(Visual.Opacity), OpacityAnimation);
                };
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", BackgroundImage);
            base.OnNavigatingFrom(e);
        }
    }
}