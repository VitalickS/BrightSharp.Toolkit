using BrightSharp.Themes;
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
using System.Windows.Shapes;

namespace BrightSharp.Ui.Tests
{
    /// <summary>
    /// Interaction logic for TabbedMainWindow.xaml
    /// </summary>
    public partial class TabbedMainWindow : Window
    {
        public TabbedMainWindow()
        {
            InitializeComponent();
            StyleNameTextBlock.Text = ThemeManager.Theme.ToString();
        }
        private void ChangeStyleButton_Click(object sender, RoutedEventArgs e)
        {
            if (Enum.IsDefined(typeof(ColorThemes), ThemeManager.Theme + 1))
                ThemeManager.Theme = ThemeManager.Theme + 1;
            else
                ThemeManager.Theme = ColorThemes.Classic;
            StyleNameTextBlock.Text = ThemeManager.Theme.ToString();
        }
        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            var cal = (Calendar)sender;
            cal.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8)));
            cal.DisplayDateStart = DateTime.Now.AddDays(-400);
        }
    }
}
