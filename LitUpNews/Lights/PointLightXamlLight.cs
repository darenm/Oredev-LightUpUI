using System;
using System.Numerics;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace CustomMayd.Controls.TextShimmer.Lights
{
    public class PointLightXamlLight : XamlLight
    {
        public static readonly DependencyProperty IntensityProperty = DependencyProperty.Register(
            "Intensity", typeof(double), typeof(PointLightXamlLight), new PropertyMetadata(1.0));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(PointLightXamlLight), new PropertyMetadata(Colors.White));

        // Register an attached property that enables apps to set a UIElement or Brush as a target for this light type in markup.
        public static readonly DependencyProperty IsTargetProperty =
            DependencyProperty.RegisterAttached(
                "IsTarget",
                typeof(bool),
                typeof(PointLightXamlLight),
                new PropertyMetadata(null, OnIsTargetChanged)
            );

        private ExpressionAnimation _lightPositionExpression;

        private Vector3KeyFrameAnimation _offsetAnimation;

        public double Intensity
        {
            get => (double)GetValue(IntensityProperty);
            set => SetValue(IntensityProperty, value);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static void SetIsTarget(DependencyObject target, bool value)
        {
            target.SetValue(IsTargetProperty, value);
        }

        public static bool GetIsTarget(DependencyObject target)
        {
            return (bool)target.GetValue(IsTargetProperty);
        }

        // Handle attached property changed to automatically target and untarget UIElements and Brushes.
        private static void OnIsTargetChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var isAdding = (bool)e.NewValue;

            if (isAdding)
            {
                if (obj is UIElement)
                    AddTargetElement(GetIdStatic(), obj as UIElement);
                else if (obj is Brush)
                    AddTargetBrush(GetIdStatic(), obj as Brush);
            }
            else
            {
                if (obj is UIElement)
                    RemoveTargetElement(GetIdStatic(), obj as UIElement);
                else if (obj is Brush)
                    RemoveTargetBrush(GetIdStatic(), obj as Brush);
            }
        }

        protected override void OnConnected(UIElement newElement)
        {
            // OnConnected is called when the first target UIElement is shown on the screen. This enables delaying composition object creation until it's actually necessary.
            var pointLight = Window.Current.Compositor.CreatePointLight();
            pointLight.Color = Color;
            pointLight.Intensity = (float)Intensity;

            // Associate CompositionLight with XamlLight
            CompositionLight = pointLight;

            // Define resting position Animation
            var restingPosition = new Vector3(200, 200, 400);
            var cbEasing =
                Window.Current.Compositor.CreateCubicBezierEasingFunction(new Vector2(0.3f, 0.7f),
                    new Vector2(0.9f, 0.5f));
            _offsetAnimation = Window.Current.Compositor.CreateVector3KeyFrameAnimation();
            _offsetAnimation.InsertKeyFrame(1, restingPosition, cbEasing);
            _offsetAnimation.Duration = TimeSpan.FromSeconds(0.5f);

            pointLight.Offset = restingPosition;

            // Define expression animation that relates light's offset to pointer position 
            var hoverPosition = ElementCompositionPreview.GetPointerPositionPropertySet(newElement);
            _lightPositionExpression =
                Window.Current.Compositor.CreateExpressionAnimation(
                    "Vector3(hover.Position.X, hover.Position.Y, height)");
            _lightPositionExpression.SetReferenceParameter("hover", hoverPosition);
            _lightPositionExpression.SetScalarParameter("height", 200.0f);

            // Configure pointer entered/ exited events
            newElement.PointerMoved += TargetElement_PointerMoved;
            newElement.PointerExited += TargetElement_PointerExited;
        }

        private void MoveToRestingPosition()
        {
            // Start animation on SpotLight's Offset 
            CompositionLight?.StartAnimation("Offset", _offsetAnimation);
        }

        private void TargetElement_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (CompositionLight == null) return;

            // touch input is still UI thread-bound as of the Creators Update
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
            {
                var offset = e.GetCurrentPoint((UIElement)sender).Position.ToVector2();
                (CompositionLight as SpotLight).Offset = new Vector3(offset.X, offset.Y, 15);
            }
            else
            {
                // Get the pointer's current position from the property and bind the SpotLight's X-Y Offset
                CompositionLight.StartAnimation("Offset", _lightPositionExpression);
            }
        }

        private void TargetElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // Move to resting state when pointer leaves targeted UIElement
            MoveToRestingPosition();
        }

        protected override void OnDisconnected(UIElement oldElement)
        {
            // OnDisconnected is called when there are no more target UIElements on the screen. The CompositionLight should be disposed when no longer required.
            CompositionLight.Dispose();
            CompositionLight = null;
            _lightPositionExpression.Dispose();
            _offsetAnimation.Dispose();
        }

        protected override string GetId()
        {
            return GetIdStatic();
        }

        private static string GetIdStatic()
        {
            // This specifies the unique name of the light. In most cases you should use the type's FullName.
            return typeof(PointLightXamlLight).FullName;
        }
    }
}