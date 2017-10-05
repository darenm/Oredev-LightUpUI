using System;
using System.Numerics;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace CreatorsNews
{
    /// <summary>
    ///     Class ContentShimmerControl. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentControl" />
    /// <inheritdoc />
    /// <seealso cref="T:Windows.UI.Xaml.Controls.ContentControl" />
    //[ContentProperty(Name = "Text")]
    public sealed class ContentShimmerControl : ContentControl
    {
        /// <summary>
        ///     The highlight color property
        /// </summary>
        public static readonly DependencyProperty HighlightColorProperty = DependencyProperty.Register(
            "HighlightColor", typeof(Color), typeof(ContentShimmerControl), new PropertyMetadata(Colors.White));

        /// <summary>
        ///     Gets or sets the color of the highlight.
        /// </summary>
        /// <value>The color of the highlight.</value>
        public Color HighlightColor
        {
            get => (Color)GetValue(HighlightColorProperty);
            set => SetValue(HighlightColorProperty, value);
        }

        /// <summary>
        ///     The intensity property
        /// </summary>
        public static readonly DependencyProperty IntensityProperty = DependencyProperty.Register(
            "Intensity", typeof(double), typeof(ContentShimmerControl), new PropertyMetadata(default(double)));

        /// <summary>
        ///     Gets or sets the intensity.
        /// </summary>
        /// <value>The intensity.</value>
        public double Intensity
        {
            get => (double)GetValue(IntensityProperty);
            set => SetValue(IntensityProperty, value);
        }

        /// <summary>
        ///     The light height property
        /// </summary>
        public static readonly DependencyProperty LightHeightProperty = DependencyProperty.Register(
            "LightHeight", typeof(double), typeof(ContentShimmerControl), new PropertyMetadata(100.0));

        /// <summary>
        ///     Gets or sets the height of the light.
        /// </summary>
        /// <value>The height of the light.</value>
        public double LightHeight
        {
            get => (double)GetValue(LightHeightProperty);
            set => SetValue(LightHeightProperty, value);
        }

        /// <summary>
        ///     The compositor
        /// </summary>
        private Compositor _compositor;

        /// <summary>
        ///     The content visual
        /// </summary>
        private Visual _contentVisual;

        /// <summary>
        ///     The light position expression
        /// </summary>
        private ExpressionAnimation _lightPositionExpression;

        /// <summary>
        ///     The offset animation
        /// </summary>
        private ScalarKeyFrameAnimation _offsetAnimation;

        /// <summary>
        ///     The point light
        /// </summary>
        private PointLight _pointLight;

        /// <summary>
        ///     The part content control
        /// </summary>
        private ContentPresenter PART_ContentControl;

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:CustomMayd.Controls.TextShimmer.ContentShimmerControl" /> class.
        /// </summary>
        public ContentShimmerControl()
        {
            DefaultStyleKey = typeof(ContentShimmerControl);
            Loaded += ShimmerTextBlock_Loaded;
            Unloaded += ShimmerTextBlock_Unloaded;
            PointerEntered += ShimmerTextBlock_PointerEntered;
            PointerExited += ShimmerTextBlock_PointerExited;
        }

        /// <summary>
        ///     Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In
        ///     simplest terms, this means the method is called just before a UI element displays in your app. Override this method
        ///     to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            PART_ContentControl = (ContentPresenter)GetTemplateChild(nameof(PART_ContentControl));
            InitializeComposition();
            base.OnApplyTemplate();
        }

        /// <summary>
        ///     Creates the pointer following animation.
        /// </summary>
        private void CreatePointerFollowingAnimation()
        {
            // Define expression animation that relates light's offset to pointer position 
            var hoverPosition = ElementCompositionPreview.GetPointerPositionPropertySet(PART_ContentControl);
            _lightPositionExpression =
                Window.Current.Compositor.CreateExpressionAnimation(
                    "Vector3(hover.Position.X, hover.Position.Y, height)");
            _lightPositionExpression.SetReferenceParameter("hover", hoverPosition);
            _lightPositionExpression.SetScalarParameter("height", (float)LightHeight);

            // Configure pointer entered/ exited events
            PART_ContentControl.PointerMoved += ShimmerTextBlock_PointerMoved;
        }

        /// <summary>
        ///     Creates the repeating animation.
        /// </summary>
        private void CreateRepeatingAnimation()
        {
            //starts out to the left; vertically centered; light's z-offset is related to fontsize
            _pointLight.Offset = new Vector3(-(float)PART_ContentControl.ActualWidth,
                (float)PART_ContentControl.ActualHeight / 2, (float)LightHeight);

            //simple offset.X animation that runs forever
            _offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _offsetAnimation.InsertKeyFrame(1, 2 * (float)ActualWidth);
            _offsetAnimation.Duration = TimeSpan.FromSeconds(3.3f);
            _offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _pointLight.StartAnimation("Offset.X", _offsetAnimation);
        }

        /// <summary>
        ///     Initializes the composition items.
        /// </summary>
        private void InitializeComposition()
        {
            _compositor = Window.Current.Compositor;
            _contentVisual = ElementCompositionPreview.GetElementVisual(PART_ContentControl);
            _pointLight = _compositor.CreatePointLight();
            _pointLight.Color = HighlightColor;
            _pointLight.CoordinateSpace = _contentVisual; //set up co-ordinate space for offset
            _pointLight.Targets.Add(_contentVisual); //target XAML ContentControl

            var ambientLight = Window.Current.Compositor.CreateAmbientLight();
            ambientLight.Color = HighlightColor;
            ambientLight.Intensity = (float)Intensity;
            ambientLight.Targets.Add(_contentVisual);
        }

        /// <summary>
        ///     Handles the Loaded event of the ContentShimmerControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ShimmerTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            CreateRepeatingAnimation();
        }

        /// <summary>
        ///     Handles the PointerEntered event of the ContentShimmerControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
        private void ShimmerTextBlock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            StopRepeatingAnimation();
            CreatePointerFollowingAnimation();
        }

        /// <summary>
        ///     Handles the PointerExited event of the ContentShimmerControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
        private void ShimmerTextBlock_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PointerMoved -= ShimmerTextBlock_PointerMoved;
            StopPointerFollowingAnimation();
            CreateRepeatingAnimation();
        }

        /// <summary>
        ///     Handles the PointerMoved event of the ContentShimmerControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
        private void ShimmerTextBlock_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_pointLight == null) return;

            // touch input is still UI thread-bound as of the Creators Update
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
            {
                var offset = e.GetCurrentPoint((UIElement)sender).Position.ToVector2();
                _pointLight.Offset = new Vector3(offset.X, offset.Y, (float)LightHeight);
            }
            else
            {
                // Get the pointer's current position from the property and bind the SpotLight's X-Y Offset
                _pointLight.StartAnimation("Offset", _lightPositionExpression);
            }
        }

        /// <summary>
        ///     Handles the Unloaded event of the ContentShimmerControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ShimmerTextBlock_Unloaded(object sender, RoutedEventArgs e)
        {
            _pointLight.Targets.Remove(_contentVisual);
            _pointLight.Dispose();
            _pointLight = null;
            _contentVisual?.Dispose();
            _lightPositionExpression?.Dispose();
            _offsetAnimation?.Dispose();
        }

        /// <summary>
        ///     Stops the pointer following animation.
        /// </summary>
        private void StopPointerFollowingAnimation()
        {
            _lightPositionExpression.Dispose();
        }

        /// <summary>
        ///     Stops the repeating animation.
        /// </summary>
        private void StopRepeatingAnimation()
        {
            _pointLight.StopAnimation("Offset.X");
            _offsetAnimation.Dispose();
        }
    }
}