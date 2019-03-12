using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TimeTracker.Interfaces;
using TimeTracker.Models.Replicon.RepliconReply;
using TimeTracker.ViewModels;
using Xamarin.Forms;

namespace TimeTracker.Helpers
{
    public static class Utils
    {
        public static TimeSpan RoundToNearestMinutes(this TimeSpan input, int minutes)
        {
            

            var totalMinutes = (int)(input + new TimeSpan(0, minutes / 2, 0)).TotalMinutes;
           

            var x = new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
            return x;
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

        public static bool IsProject(this Row row)
        {
            return row.task == null;
        }

        public static bool IsProject(this ITimeEntryListElement element)
        {
            return string.IsNullOrEmpty(element?.Ticket?.description);
        }


        public static TimeSpan ParseToDateTime(this string entry)
        {
            //attempt to parse string as a double of total hours
            var validParse = double.TryParse(entry, out double doubleParseResult);

            TimeSpan enteredTimeSpan;
            //if passed get timespan from entry
            if (validParse)
            {
                enteredTimeSpan = TimeSpan.FromHours(doubleParseResult);
            }
            //if failed, attempt to parse string as a  timespan
            else
            {
                validParse = TimeSpan.TryParse(entry, out enteredTimeSpan);
            }

            //parsing successful, correct time
            if (validParse)
            {
                return enteredTimeSpan;
            }
            return TimeSpan.MinValue;
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


    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? (Color)Application.Current.Resources["AbasGreen"] :(Color) Application.Current.Resources["AbasRed"];
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            // return ((string) value).Equals("Bill");
        }
    }

    public class NullToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Color.LightCoral : Color.White;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            // return ((string) value).Equals("Bill");
        }
    }

    public class NullToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            // return ((string) value).Equals("Bill");
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
