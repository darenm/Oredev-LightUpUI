using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using CustomMayd.Controls.TextShimmer.Lights;

namespace CustomMayd.Controls.TextShimmer.Lights
{
    public class AmbientLightXamlLight : XamlLight
    {
        public static readonly DependencyProperty IntensityProperty = DependencyProperty.Register(
            "Intensity", typeof(double), typeof(AmbientLightXamlLight), new PropertyMetadata(1.0));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(AmbientLightXamlLight), new PropertyMetadata(Colors.White));

        // Register an attached property that enables apps to set a UIElement or Brush as a target for this light type in markup.
        public static readonly DependencyProperty IsTargetProperty =
            DependencyProperty.RegisterAttached(
                "IsTarget",
                typeof(bool),
                typeof(SpotLightXamlLight),
                new PropertyMetadata(null, OnIsTargetChanged)
            );

        public double Intensity
        {
            get => (double) GetValue(IntensityProperty);
            set => SetValue(IntensityProperty, value);
        }

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }


        public static void SetIsTarget(DependencyObject target, bool value)
        {
            target.SetValue(IsTargetProperty, value);
        }

        public static bool GetIsTarget(DependencyObject target)
        {
            return (bool) target.GetValue(IsTargetProperty);
        }

        // Handle attached property changed to automatically target and untarget UIElements and Brushes.
        private static void OnIsTargetChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var isAdding = (bool) e.NewValue;

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
            var ambientLight = Window.Current.Compositor.CreateAmbientLight();
            ambientLight.Color = Color;
            ambientLight.Intensity = (float)Intensity;

            // Associate CompositionLight with XamlLight
            CompositionLight = ambientLight;
        }

        protected override void OnDisconnected(UIElement oldElement)
        {
            // OnDisconnected is called when there are no more target UIElements on the screen. The CompositionLight should be disposed when no longer required.
            CompositionLight.Dispose();
            CompositionLight = null;
        }

        protected override string GetId()
        {
            return GetIdStatic();
        }

        private static string GetIdStatic()
        {
            // This specifies the unique name of the light. In most cases you should use the type's FullName.
            return typeof(SpotLightXamlLight).FullName;
        }
    }
}