using Aftermath.Models.Characters.States;
using AftermathModels.Occupation;
using System.Globalization;
using System.Windows.Data;

namespace Aftermath.UI.Converters
{
    public class StatusImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return $"pack://application:,,,/Resources/Icons/{(Status)value}Status.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
