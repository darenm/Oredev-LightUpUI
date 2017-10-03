using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Shared
{
    public class FirstLastDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate FirstItemTemplate { get; set; }

        public static ListViewBase GetListView(DependencyObject element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            if (parent == null)
            {
                return null;
            }
            var parentGridView = parent as ListViewBase;
            return parentGridView ?? GetListView(parent);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var lv = GetListView(container);
            var i = lv?.Items?.IndexOf(item);
            if (i == 0)
            {
                return FirstItemTemplate;
            }
            return DefaultTemplate;
        }
    }
}