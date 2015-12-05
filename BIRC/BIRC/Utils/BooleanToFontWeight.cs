using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;

namespace BIRC.Utils
{
    public class BooleanToFontWeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int res = (int)value;

            if (res > 0)
                return FontWeights.Bold;
            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
