using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CreatorsNews.Models;
using CreatorsNews.Views;
using Windows.ApplicationModel.Core;

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
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;

            HamburgerMenuControl.ItemsSource = MenuItem.GetMainItems();
            HamburgerMenuControl.OptionsItemsSource = MenuItem.GetOptionsItems();
            HamburgerMenuControl.SelectedIndex = 0;
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
                RootFrame.GoBack();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested +=
                App_BackRequested;
            RootFrame.NavigationFailed += OnNavigationFailed;
            RootFrame.Navigate(typeof(HomePage));
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
    }
}