using AppleJobs.Data.Models.ModelsJobs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JetFrames.AppleJobs.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = App.Locator;
            InitializeComponent();
            Loaded += (s, e) => App.Locator.Editor.OnLoaded();
        }

        private void DataGrid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var dg = (DataGrid)sender;

            if (e.Command == DataGrid.DeleteCommand)
                e.Handled = App.Locator.Editor.DeleteEntity(dg.SelectedValue, true);
        }

        private void DataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            if (e.NewItem is ModelJobPriceTemplate)
            {
                var mjpt = (ModelJobPriceTemplate)e.NewItem;
                mjpt.Customers_Id = 1;
            }
            App.Locator.Editor.AddEntity(e.NewItem);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((TabItem)((TabControl)sender).SelectedItem).DataContext = App.Locator;
        }
    }
}
