using System;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CreatorsNews.Controls
{
    public partial class NavProperties : DependencyObject
    {
        public static Type GetPageType(NavigationViewItem obj)
            => (Type)obj.GetValue(PageTypeProperty);
        public static void SetPageType(NavigationViewItem obj, Type value)
            => obj.SetValue(PageTypeProperty, value);
        public static readonly DependencyProperty PageTypeProperty =
            DependencyProperty.RegisterAttached("PageType", typeof(Type),
                typeof(NavProperties), new PropertyMetadata(null));

        public static bool GetIsStartPage(NavigationViewItem obj)
            => (bool)obj.GetValue(IsStartPageProperty);
        public static void SetIsStartPage(NavigationViewItem obj, bool value)
            => obj.SetValue(IsStartPageProperty, value);
        public static readonly DependencyProperty IsStartPageProperty =
            DependencyProperty.RegisterAttached("IsStartPage", typeof(bool),
                typeof(NavProperties), new PropertyMetadata(false));

        public static string GetHeader(Page obj)
            => (string)obj.GetValue(HeaderProperty);
        public static void SetHeader(Page obj, string value)
            => obj.SetValue(HeaderProperty, value);
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(string),
                typeof(NavProperties), new PropertyMetadata(null));
    }

    public class NavViewEx : NavigationView
    {
        Frame _frame;

        public Type SettingsPageType { get; set; }

        public NavViewEx()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += ShellPage_BackRequested;
            RegisterPropertyChangedCallback(IsPaneOpenProperty, IsPaneOpenChanged);
            Loaded += (s, e) =>
            {
                _frame = FindDescendant<Frame>((FrameworkElement)Content);
                if (_frame == null)
                {
                    Content =_frame = new Frame();
                }
                _frame.Navigated += Frame_Navigated;
                ItemInvoked += NavViewEx_ItemInvoked;

                if (FindStart() is NavigationViewItem i)
                {
                    Navigate(i.GetValue(NavProperties.PageTypeProperty) as Type);
                }
            };
        }


        private void IsPaneOpenChanged(DependencyObject sender, DependencyProperty dp)
        {
            foreach (var item in MenuItems.OfType<NavigationViewItemHeader>())
            {
                item.Opacity = IsPaneOpen ? 1 : 0;
            }
        }

        private void NavViewEx_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
            => SelectedItem = (args.IsSettingsInvoked) ? SettingsItem : Find(args.InvokedItem.ToString()) ?? base.SelectedItem;

        private void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            SelectedItem = (e.SourcePageType == SettingsPageType) ? SettingsItem : Find(e.SourcePageType) ?? base.SelectedItem;
            UpdateHeader();
        }

        private void UpdateHeader()
        {
            if (_frame.Content is Page p && p.GetValue(NavProperties.HeaderProperty) is string s && !string.IsNullOrEmpty(s))
            {
                Header = s;
            }
        }

        private void ShellPage_BackRequested(object sender, BackRequestedEventArgs e)
            => _frame.GoBack();

        NavigationViewItem FindStart()
            => MenuItems.OfType<NavigationViewItem>().SingleOrDefault(x => (bool)x.GetValue(NavProperties.IsStartPageProperty));

        NavigationViewItem Find(string content)
            => MenuItems.OfType<NavigationViewItem>().SingleOrDefault(x => x.Content.Equals(content));

        NavigationViewItem Find(Type type)
            => MenuItems.OfType<NavigationViewItem>().SingleOrDefault(x => type.Equals(x.GetValue(NavProperties.PageTypeProperty)));

        public bool Navigate(Type type)
            => Navigate(_frame, type);

        public virtual bool Navigate(Frame frame, Type type)
            => frame.Navigate(type);

        public new object SelectedItem
        {
            set
            {
                if (value != base.SelectedItem)
                {
                    if (value == SettingsItem)
                    {
                        if (SettingsPageType != null)
                        {
                            Navigate(SettingsPageType);
                            base.SelectedItem = value;
                            _frame.BackStack.Clear();
                        }
                        //SettingsInvoked?.Invoke(this, EventArgs.Empty);
                    }
                    else if (value == base.SelectedItem)
                    {
                        // do nothing
                    }
                    else if (value is NavigationViewItem i && i != null)
                    {
                        Navigate(i.GetValue(NavProperties.PageTypeProperty) as Type);
                        base.SelectedItem = value;
                        _frame.BackStack.Clear();
                    }
                }
                UpdateBackButton();
                UpdateHeader();
            }
        }
        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                (_frame.CanGoBack) ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// Find first descendant control of a specified type.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="element">Parent element.</param>
        /// <returns>Descendant control or null if not found.</returns>
        private static T FindDescendant<T>(DependencyObject element)
            where T : DependencyObject
        {
            T retValue = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is T type)
                {
                    retValue = type;
                    break;
                }

                retValue = FindDescendant<T>(child);

                if (retValue != null)
                {
                    break;
                }
            }

            return retValue;
        }
    }
}
