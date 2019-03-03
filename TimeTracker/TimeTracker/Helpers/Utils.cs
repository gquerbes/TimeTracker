﻿using System;
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
