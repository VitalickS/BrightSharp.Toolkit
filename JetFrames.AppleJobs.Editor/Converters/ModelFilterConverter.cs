using AppleJobs.Data.Models.ModelsJobs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JetFrames.AppleJobs.Editor.Converters
{
    public class ModelFilterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var initCollection = (IEnumerable<ModelJob>)values[0];
            var modelJob = (ModelJob)values[1];
            if (modelJob == null) return initCollection;
            return initCollection.Where(mj => mj.Models_Id == modelJob.Models_Id);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
