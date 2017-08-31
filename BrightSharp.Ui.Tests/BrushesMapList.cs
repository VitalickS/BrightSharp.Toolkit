using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BrightSharp.Ui.Tests
{
    public class BrushesMapList : Dictionary<string, object>
    {
        public BrushesMapList() {
            var dic = Application.Current.Resources.MergedDictionaries.First();
            foreach (string key in dic.Keys.OfType<string>().OrderBy(x => x)) {
                var value = Application.Current.TryFindResource(key);
                if (value != null) {
                    bool isBorder = key.Contains("Border");
                    if (value is Color col) {
                        Add(key, new { Type = "C", Brush = new SolidColorBrush(col), IsBorder = isBorder });
                    }
                    else if (value is Brush br) {
                        Add(key, new { Type = "Br", Brush = br, IsBorder = isBorder });
                    }
                }
            }
        }
    }
}
