using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JetFrames.AppleJobs.Editor.Views
{
    /// <summary>
    /// Interaction logic for Models.xaml
    /// </summary>
    public partial class Models : UserControl
    {
        public Models()
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
