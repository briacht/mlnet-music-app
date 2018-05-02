using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IntelligentDemo.Pages
{
    /// <summary>
    /// Interaction logic for SplitPage.xaml
    /// </summary>
    public partial class SplitPage : UserControl
    {
        public SplitPage(IEnumerable<UserControl> pages)
        {
            InitializeComponent();

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var a = pages.ToArray();

            var rows = a.Length / 2 + a.Length % 2;

            for (int i = 0; i < rows; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());

                var left = new ContentControl { Content = a[i * 2] };
                left.SetValue(Grid.RowProperty, i);
                left.SetValue(Grid.ColumnProperty, 0);
                MainGrid.Children.Add(left);

                if (i * 2 + 1 < a.Length)
                {
                    var right = new ContentControl { Content = a[i * 2 + 1] };
                    right.SetValue(Grid.RowProperty, i);
                    right.SetValue(Grid.ColumnProperty, 1);
                    MainGrid.Children.Add(right);
                }
            }
        }
    }
}
