using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TimeTracker.Helpers
{
    public static class Utils
    {
        public static TimeSpan RoundToNearestMinutes(this TimeSpan input, int minutes)
        {
            var totalMinutes = (int)(input + new TimeSpan(0, minutes / 2, 0)).TotalMinutes;

            if (totalMinutes > 5 && totalMinutes < 15)
            {
                totalMinutes = 15;
            }

            return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
        }

        public static string NormalizeIntForTime(this int input)
        {
            if (input < 10)
            {
                return $"0{input}";
            }
            else
            {
                return input.ToString();
            }
        }
    }


   
    public class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }



    public class BoolToBillTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Bill" : "Non-Bill";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            // return ((string) value).Equals("Bill");
        }
    }
}
