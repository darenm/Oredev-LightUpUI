using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Shared
{
    public class VariableSizedGridView : GridView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element,
            object item)
        {
            try
            {
                dynamic localItem = item;
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, localItem.RowSpan);
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, localItem.ColSpan);
            }
            catch
            {
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
            }

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}