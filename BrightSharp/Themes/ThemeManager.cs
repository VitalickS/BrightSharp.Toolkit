using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private const string StaticThemeDictionaryUri = "/brightsharp;component/themes/theme.static.xaml";

        static ColorThemes? _theme;

        public static ColorThemes? Theme
        {
            get {
                if (_theme.HasValue) return _theme.Value;
                var curStyleRes = Resources.Where(r => r.Source != null &&
                    Regex.IsMatch(r.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase)).FirstOrDefault();
                if (curStyleRes == null) return null;
                var match = Regex.Match(curStyleRes.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase);
                _theme = (ColorThemes)Enum.Parse(typeof(ColorThemes), match.Value, true);
                return _theme.Value;
            }
            set {
                SetTheme(value);
            }
        }

        public static DispatcherOperation SetTheme(ColorThemes? value, DispatcherPriority priority = DispatcherPriority.Background) {
            if (_theme == value) return Application.Current.Dispatcher.BeginInvoke(new Action(() => { }), priority);
            _theme = value;
            return Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_theme != value) return;
                Application.Current.Resources.BeginInit();

                var curStyleRes = Resources.Where(r => r.Source != null &&
                        Regex.IsMatch(r.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase)).FirstOrDefault();
                var curThemeRes = Resources.Where(r => r.Source != null && r.Source.OriginalString.Equals(StaticThemeDictionaryUri, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (curThemeRes != null)
                    Resources.Remove(curThemeRes);
                if (curStyleRes != null)
                    Resources.Remove(curStyleRes);
                if (value == null) return;
                Resources.Add(new ResourceDictionary() { Source = new Uri($"/brightsharp;component/themes/style.{value}.xaml", UriKind.RelativeOrAbsolute) });
                if (curThemeRes != null)
                    Resources.Add(new ResourceDictionary() { Source = new Uri(StaticThemeDictionaryUri, UriKind.RelativeOrAbsolute) });

                Application.Current.Resources.EndInit();
                ThemeChanged?.Invoke(null, EventArgs.Empty);
            }), priority);
        }

        public static event EventHandler ThemeChanged;

        private static ICollection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }
    }

}
