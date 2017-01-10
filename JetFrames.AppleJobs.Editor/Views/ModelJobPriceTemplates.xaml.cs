using System.Windows.Controls;

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
            EditorViewModel.InitGrid(dg);
        }
    }
}
