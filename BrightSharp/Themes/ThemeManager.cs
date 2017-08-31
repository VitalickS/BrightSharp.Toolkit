using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

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
        public static ColorThemes Theme
        {
            get
            {
                var curStyleRes = Resources.Where(r => r.Source != null &&
                        Regex.IsMatch(r.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase)).FirstOrDefault();
                if (curStyleRes == null) return ColorThemes.Classic;
                var match = Regex.Match(curStyleRes.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase);
                return (ColorThemes)Enum.Parse(typeof(ColorThemes), match.Value, true);
            }
            set
            {
                var curStyleRes = Resources.Where(r => r.Source != null &&
                        Regex.IsMatch(r.Source.OriginalString, StyleDictionaryPattern, RegexOptions.IgnoreCase)).FirstOrDefault();
                if (curStyleRes != null)
                    Resources.Remove(curStyleRes);
                if (value == ColorThemes.Classic)
                    return;
                Resources.Add(new ResourceDictionary()
                {
                    Source = new Uri($"/brightsharp;component/themes/style.{value}.xaml", UriKind.RelativeOrAbsolute)
                });
            }
        }
        private static ICollection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }
    }

}
