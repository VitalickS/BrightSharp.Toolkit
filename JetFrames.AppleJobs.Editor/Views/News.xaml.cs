using System.Windows.Controls;
using System.Windows.Input;

namespace JetFrames.AppleJobs.Editor.Views
{
    /// <summary>
    /// Interaction logic for News.xaml
    /// </summary>
    public partial class News : UserControl
    {
        public News()
        {
            InitializeComponent();
            EditorViewModel.InitGrid(dg);
        }
    }
}
