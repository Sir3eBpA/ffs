using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FFS.Converters
{
    [ValueConversion(typeof(string), typeof(Bitmap))]
    public class ExtensionToIconConverter : IValueConverter
    {
        private Dictionary<string, BitmapSource> _iconsCache = new Dictionary<string, BitmapSource>();

        #region IValueConverter Members

        [SupportedOSPlatform("windows")]
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string inData = (string)value;
            if (string.IsNullOrEmpty(inData))
                return null;

            if (File.Exists(inData) && Path.HasExtension(inData.AsSpan()))
            {
                string ex = Path.GetExtension(inData);
                if (_iconsCache.TryGetValue(ex, out BitmapSource cachedIcon))
                    return cachedIcon;

                using Icon ico = Icon.ExtractAssociatedIcon(inData);
                BitmapSource icon = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                _iconsCache.Add(ex, icon);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

#endregion
    }
}
