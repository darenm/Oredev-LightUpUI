using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CreatorsNews.Models;
using CreatorsNews.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CreatorsNews
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            //HamburgerMenuControl.ItemsSource = MenuItem.GetMainItems();
            //HamburgerMenuControl.OptionsItemsSource = MenuItem.GetOptionsItems();
            //HamburgerMenuControl.SelectedIndex = 0;

            //draw into the title bar
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            // Note - a different type of TitleBar...
            CoreApplicationViewTitleBar coreApplicationViewTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreApplicationViewTitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;


            Loaded += OnLoaded;
        }

        private void App_BackRequested(object sender,
            BackRequestedEventArgs e)
        {
            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (RootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                var targetPage = RootFrame.BackStack.Last().SourcePageType;

                RootFrame.GoBack();

                string desiredTag = string.Empty;
                if (targetPage == typeof(HomePage))
                {
                    desiredTag = "Home";
                }
                else if (targetPage == typeof(NewsPage))
                {
                    desiredTag = "News";
                }
                else if (targetPage == typeof(ArticlesPage))
                {
                    desiredTag = "Games";
                }
                else if (targetPage == typeof(SettingsPage))
                {
                    desiredTag = "Settings";
                }

                SetSelectedMenuItem(desiredTag);
            }
        }
        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitle.Margin = new Thickness(CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset + 12, 8, 0, 0);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested +=
                App_BackRequested;
            RootFrame.NavigationFailed += OnNavigationFailed;
            RootFrame.Navigated += RootFrameOnNavigated;
            SetSelectedMenuItem("Home");
        }

        private void SetSelectedMenuItem(string tag)
        {
            foreach (NavigationViewItem item in NavView.MenuItems)
            {
                if (item.Tag.ToString() == tag)
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }
        }

        private void RootFrameOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        ///     Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            RootFrame.Navigate(menuItem.PageType);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type targetType = null;
            var currentPageType = RootFrame.Content?.GetType();

            if (args.IsSettingsSelected)
            {
                DetermineValidNavigation(currentPageType, typeof(SettingsPage), out targetType);
            }
            else
            {

                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag)
                {
                    case "Home":
                        DetermineValidNavigation(currentPageType, typeof(HomePage), out targetType);
                        break;

                    case "Games":
                        DetermineValidNavigation(currentPageType, typeof(ArticlesPage), out targetType);
                        break;

                    case "News":
                        DetermineValidNavigation(currentPageType, typeof(NewsPage), out targetType);
                        break;
                }
            }

            if (targetType != null)
            {
                RootFrame.Navigate(targetType);
            }
        }

        private void DetermineValidNavigation(Type currentPageType, Type desiredTargetType, out Type actualTargetType)
        {
            actualTargetType = null;
            if (currentPageType == null || currentPageType != desiredTargetType)
            {
                actualTargetType = desiredTargetType;
            }
        }
    }
}