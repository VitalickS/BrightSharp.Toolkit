using BrightSharp.Diagrams;
using BrightSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrightSharp.Ui.Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SelectionBehavior.Attach(innerCanvas);
            tb.Text = ThemeManager.Theme.ToString();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Enum.IsDefined(typeof(ColorThemes), ThemeManager.Theme + 1))
            {
                ThemeManager.Theme = ThemeManager.Theme + 1;
            }
            else
            {
                ThemeManager.Theme = ColorThemes.Classic;
            }
            tb.Text = ThemeManager.Theme.ToString();
        }
    }
}
