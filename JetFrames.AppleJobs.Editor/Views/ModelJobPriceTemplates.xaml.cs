using AppleJobs.Data.Models.ModelsJobs;
using System.Windows.Controls;
using System.Windows.Input;

namespace JetFrames.AppleJobs.Editor.Views
{
    /// <summary>
    /// Interaction logic for ModelJobPriceTemplates.xaml
    /// </summary>
    public partial class ModelJobPriceTemplates : UserControl
    {
        public ModelJobPriceTemplates()
        {
            InitializeComponent();
        }
        private void DataGrid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == DataGrid.DeleteCommand)
                e.Handled = App.Locator.Editor.DeleteEntity(((DataGrid)sender).SelectedItems, true);
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
    }
}
