using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace Starving.Converters
{

    public class ByteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return ConvertByteArrayToBitMapImage(value as byte[]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotImplementedException();
        }

        private ImageSource ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        {
            return ImageSource.FromStream(() => new MemoryStream(imageByteArray));
        }
    }
}