using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
    public sealed partial class NewsArticlePage : Page
    {
        public NewsArticlePage()
        {
            this.InitializeComponent();

            var opacityAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            opacityAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
            opacityAnimation.DelayTime = TimeSpan.FromMilliseconds(100);
            opacityAnimation.Duration = TimeSpan.FromMilliseconds(500);
            opacityAnimation.InsertKeyFrame(0, 0);
            opacityAnimation.InsertKeyFrame(1, 1);
            opacityAnimation.Target = nameof(Visual.Opacity);
            ElementCompositionPreview.SetImplicitShowAnimation(ArticleBody, opacityAnimation);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var articleIndex = (int)e.Parameter;
                ViewModel.LoadArticle(articleIndex);
            }

            BackgroundImage.Source = ViewModel.Article.MainImage;

            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Image");
            animation?.TryStart(BackgroundImage, new[] {TitleTextBlock});
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Add a fade out effect
            Transitions = new TransitionCollection { new ContentThemeTransition() };

            if (e.NavigationMode == NavigationMode.Back)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", BackgroundImage);
            }
            base.OnNavigatingFrom(e);
        }

    }
}
