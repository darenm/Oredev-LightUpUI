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
        private static Vector3KeyFrameAnimation FlyInTranslationAnimation;
        private static Vector3KeyFrameAnimation FlyOutTranslationAnimation;
        private static CompositionAnimationGroup AnimationGroup;
        private Visual _articleVisual;

        public ArticlePage()
        {
            InitializeComponent();
            EnsureAnimation();
            _articleVisual = ElementCompositionPreview.GetElementVisual(ArticlePagePanel);
            ElementCompositionPreview.SetIsTranslationEnabled(ArticlePagePanel, true);
            _articleVisual.Opacity = 0;
        }

        private void EnsureAnimation()
        {
            if (AnimationGroup != null)
            {
                return;
            }

            var linear = Window.Current.Compositor.CreateLinearEasingFunction();
            OpacityAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimation.InsertKeyFrame(0, 0);
            OpacityAnimation.InsertKeyFrame(0.2f, 0, linear);
            OpacityAnimation.InsertKeyFrame(1, 1, linear);
            OpacityAnimation.Duration = TimeSpan.FromMilliseconds(600);
            OpacityAnimation.Target = nameof(Visual.Opacity);

            FlyInTranslationAnimation = Window.Current.Compositor.CreateVector3KeyFrameAnimation();
            FlyInTranslationAnimation.InsertKeyFrame(0, new System.Numerics.Vector3(0, 2000, 0));
            FlyInTranslationAnimation.InsertKeyFrame(1, System.Numerics.Vector3.Zero);
            FlyInTranslationAnimation.Duration = TimeSpan.FromMilliseconds(600);
            FlyInTranslationAnimation.Target = "Translation";
            AnimationGroup = Window.Current.Compositor.CreateAnimationGroup();
            AnimationGroup.Add(OpacityAnimation);
            AnimationGroup.Add(FlyInTranslationAnimation);
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
                    _articleVisual.StartAnimationGroup(AnimationGroup);
                };
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Add a fade out effect
            Transitions = new TransitionCollection();
            Transitions.Add(new ContentThemeTransition());

            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", BackgroundImage);
            base.OnNavigatingFrom(e);
        }
    }
}