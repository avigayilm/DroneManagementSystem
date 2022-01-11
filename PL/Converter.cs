using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL
{
    internal class BatteryToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value switch
            {
                < 10 => Brushes.DarkRed,
                < 20 => Brushes.Red,
                < 40 => Brushes.Yellow,
                < 60 => Brushes.GreenYellow,
                _ => Brushes.Green
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();

    }

    internal class StatusToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value switch
            {
                BO.DroneStatuses.Available => Brushes.DarkRed,
                BO.DroneStatuses.Delivery=> Brushes.Yellow,
                BO.DroneStatuses.Maintenance=>Brushes.Orange,
                _ => Brushes.White
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();

    }

    /// <summary>
    /// checks if all textboxes have a value
    /// </summary>
    public class HasAllTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool res = true;

            foreach (object val in values)
            {
                if (string.IsNullOrEmpty(val as string))
                {
                    res = false;
                }
            }

            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
    