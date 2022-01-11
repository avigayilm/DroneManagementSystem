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
                DroneStatuses.Available => Brushes.DarkRed,
                DroneStatuses.Delivery=> Brushes.Yellow,
                DroneStatuses.Maintenance=>Brushes.Orange,
                _ => Brushes.White
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();

    }
}
    