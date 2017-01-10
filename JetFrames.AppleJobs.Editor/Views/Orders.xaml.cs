using AppleJobs.Data.Models;
using AppleJobs.Data.Models.ModelsJobs;
using AppleJobs.Data.Models.Orders;
using System.Windows.Controls;
using System.Windows.Input;

namespace JetFrames.AppleJobs.Editor.Views
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        public Orders()
        {
            InitializeComponent();
            EditorViewModel.InitGrid(dg);
        }
        
    }
}
