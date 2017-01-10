using BrightSharp;
using System.Windows;
using System.Windows.Controls;

namespace JetFrames.AppleJobs.Editor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.Theme = (ColorThemes)((FrameworkElement)e.OriginalSource).DataContext;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = (TreeViewItem)e.NewValue;
            var tabitem = (TabItem)item.Tag;
            if (tabitem != null)
            {
                if (!tabs.Items.Contains(tabitem))
                {
                    tabs.Items.Add(tabitem);
                }
                tabs.SelectedItem = tabitem;
            }
        }
    }
}
