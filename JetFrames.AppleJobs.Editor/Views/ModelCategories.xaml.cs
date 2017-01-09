using System.Windows.Controls;
using System.Windows.Input;

namespace JetFrames.AppleJobs.Editor.Views
{
    /// <summary>
    /// Interaction logic for ModelCategories.xaml
    /// </summary>
    public partial class ModelCategories : UserControl
    {
        public ModelCategories()
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
            App.Locator.Editor.AddEntity(e.NewItem);
        }
    }
}
