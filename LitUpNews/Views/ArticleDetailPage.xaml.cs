using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CreatorsNews.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticleDetailPage : Page
    {
        private static ScalarKeyFrameAnimation _opacityAnimation;
        private static Vector3KeyFrameAnimation _flyInTranslationAnimation;
        private static CompositionAnimationGroup _animationGroup;

        #region Fields

        private readonly Visual _articleVisual;

        #endregion

        public ArticleDetailPage()
        {
            InitializeComponent();
            //EnsureAnimation();
            //_articleVisual = ElementCompositionPreview.GetElementVisual(ArticlePagePanel);
            //ElementCompositionPreview.SetIsTranslationEnabled(ArticlePagePanel, true);
            //_articleVisual.Opacity = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var articleIndex = (int) e.Parameter;
                ViewModel.LoadArticle(articleIndex);
            }

            BackgroundImage.Source = ViewModel.Article.MainImage;

            //var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Image");
            //if (animation != null)
            //{
            //    BackgroundImage.TryStartAnimation(animation);
            //    _articleVisual.StartAnimationGroup(_animationGroup);
            //}
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Add a fade out effect
            Transitions = new TransitionCollection {new ContentThemeTransition()};

            //if (e.NavigationMode == NavigationMode.Back)
            //{
            //    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", BackgroundImage);
            //}
            base.OnNavigatingFrom(e);
        }

        private void EnsureAnimation()
        {
            if (_animationGroup != null)
            {
                return;
            }

            var linear = Window.Current.Compositor.CreateLinearEasingFunction();

            _opacityAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            _opacityAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
            _opacityAnimation.DelayTime = TimeSpan.FromMilliseconds(200);
            _opacityAnimation.InsertKeyFrame(0, 0);
            _opacityAnimation.InsertKeyFrame(1, 1, linear);
            _opacityAnimation.Duration = TimeSpan.FromMilliseconds(600);
            _opacityAnimation.Target = nameof(Visual.Opacity);

            _flyInTranslationAnimation = Window.Current.Compositor.CreateVector3KeyFrameAnimation();
            _flyInTranslationAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
            _flyInTranslationAnimation.DelayTime = TimeSpan.FromMilliseconds(200);
            _flyInTranslationAnimation.InsertKeyFrame(0, new Vector3(0, 2000, 0));
            _flyInTranslationAnimation.InsertKeyFrame(1, Vector3.Zero);
            _flyInTranslationAnimation.Duration = TimeSpan.FromMilliseconds(600);
            _flyInTranslationAnimation.Target = "Translation";

            _animationGroup = Window.Current.Compositor.CreateAnimationGroup();
            _animationGroup.Add(_opacityAnimation);
            _animationGroup.Add(_flyInTranslationAnimation);
        }
    }

    internal static class ImageExtensions
    {
        internal static void TryStartAnimation(this Image image, ConnectedAnimation animation)
        {
            image.Opacity = 0;

            // new C# 7 capability - local functions
            void Handler(object sender, RoutedEventArgs args)
            {
                image.Opacity = 1;
                animation.TryStart(image);
                image.ImageOpened -= Handler;
            }

            image.ImageOpened += Handler;
        }
    }
}