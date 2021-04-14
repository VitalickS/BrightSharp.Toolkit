using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BrightSharp.Themes
{
    public enum ColorThemes
    {
        Classic,
        DevLab,
        Silver,
        Blue,
        DarkBlue
    }

    public static class ThemeManager
    {
        private const string StyleDictionaryPattern = @"(?<=.+style\.)(.*?)(?=\.xaml)";
        private const string ThemeDictionaryUri = "/brightsharp;component/themes/theme.xaml";

        private static ColorThemes? ThemeField;

        public static ColorThemes? Theme
        {
            get {
                if (ThemeField.HasValue) return ThemeField.Value;
                var curStyleRes = Resources.Where(r => r.Source != null &&
                    Regex.IsMatch(r.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase)).FirstOrDefault();
                if (curStyleRes == null) return null;
                var match = Regex.Match(curStyleRes.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase);
                ThemeField = (ColorThemes)Enum.Parse(typeof(ColorThemes), match.Value, true);
                return ThemeField.Value;
            }
            set {
                _ = SetTheme(value);
            }
        }

        public static async Task SetTheme(ColorThemes? value, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (ThemeField == value) { return; }
            ThemeField = value;
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (ThemeField != value) return;
                Application.Current.Resources.BeginInit();

                var curStyleRes = Resources.Where(r => r.Source != null &&
                        Regex.IsMatch(r.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase)).FirstOrDefault();
                var curThemeRes = Resources.Where(r => r.Source != null && r.Source.OriginalString.Equals(ThemeDictionaryUri, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (curThemeRes != null)
                    Resources.Remove(curThemeRes);
                if (curStyleRes != null)
                    Resources.Remove(curStyleRes);
                if (value == null) return;
                Resources.Add(new ResourceDictionary() { Source = new Uri($"/brightsharp;component/themes/style.{value}.xaml", UriKind.RelativeOrAbsolute) });
                Resources.Add(new ResourceDictionary() { Source = new Uri(ThemeDictionaryUri, UriKind.RelativeOrAbsolute) });

                Application.Current.Resources.EndInit();
                ThemeChanged?.Invoke(null, EventArgs.Empty);
            }, priority);
        }

        public static event EventHandler ThemeChanged;

        private static ICollection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }
    }

}
