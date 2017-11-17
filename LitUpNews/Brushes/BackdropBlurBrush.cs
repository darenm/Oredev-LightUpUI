using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Effects;

namespace LitUpNews.Brushes
{
    public sealed class BackdropBlurBrush : XamlCompositionBrushBase
    {
        public static readonly DependencyProperty TintColorProperty = DependencyProperty.Register(
            "TintColor", typeof(Color), typeof(BackdropBlurBrush),
            new PropertyMetadata(Colors.Transparent, OnTintColorChanged));

        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register(
            "BlurAmount",
            typeof(double),
            typeof(BackdropBlurBrush),
            new PropertyMetadata(0.0, OnBlurAmountChanged
            )
        );

        public Color TintColor
        {
            get => (Color) GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }

        public double BlurAmount
        {
            get => (double) GetValue(BlurAmountProperty);
            set => SetValue(BlurAmountProperty, value);
        }

        private static void OnTintColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (BackdropBlurBrush) d;
            // Unbox and set a new blur amount if the CompositionBrush exists.
            brush.CompositionBrush?.Properties.InsertScalar("Tint.Color", (float) (double) e.NewValue);
        }

        private static void OnBlurAmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (BackdropBlurBrush) d;
            // Unbox and set a new blur amount if the CompositionBrush exists.
            brush.CompositionBrush?.Properties.InsertScalar("Blur.BlurAmount", (float) (double) e.NewValue);
        }

        protected override void OnConnected()
        {
            // Delay creating composition resources until they're required.
            if (CompositionBrush == null)
            {
                var backdrop = Window.Current.Compositor.CreateBackdropBrush();

                var graphicsEffect = new BlendEffect
                {
                    Mode = BlendEffectMode.ColorBurn,
                    Background = new ColorSourceEffect
                    {
                        Name = "Tint",
                        Color = TintColor
                    },

                    Foreground = new GaussianBlurEffect
                    {
                        Name = "Blur",
                        Source = new CompositionEffectSourceParameter("Backdrop"),
                        BlurAmount = (float)BlurAmount,
                        BorderMode = EffectBorderMode.Hard
                    }
                };
                var effectFactory =
                    Window.Current.Compositor.CreateEffectFactory(graphicsEffect,
                        new[] {"Blur.BlurAmount", "Tint.Color"});
                var effectBrush = effectFactory.CreateBrush();

                effectBrush.SetSourceParameter("Backdrop", backdrop);

                CompositionBrush = effectBrush;
            }
        }

        protected override void OnDisconnected()
        {
            // Dispose of composition resources when no longer in use.
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }
    }
}