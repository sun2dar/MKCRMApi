using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace CCARE.App_Function
{
    public class Formatter
    {

        public static string AccountNumber(string account) { 
            return account.Length == 10 ? account.Substring(0,3) + " - " + account.Substring(3,3) + " - " + account.Substring(6,4) : account;
        }

        public static String FormatDateHist(DateTime date1)
        {
            string[] datePart = date1.ToString().Split(' ')[0].Split('/');
            return datePart[2] + "-" + String.Format("{00}", datePart[1]) + "-" + String.Format("{00}", datePart[0]);
        }

        public static int? GetParsedNumeric(string value, bool useCurrentCultureSetting)
        {
            int parsedValue = 0;

            if (!string.IsNullOrEmpty(value))
            {
                CultureInfo culture = new CultureInfo("en-US");

                if (useCurrentCultureSetting)
                {
                    culture = CultureInfo.CurrentCulture;
                }

                if (int.TryParse(value, NumberStyles.Any, culture, out parsedValue))
                {
                    return parsedValue;
                }
            }
            return null;
        }

        public static DateTime? ParseExact(string value, string format)
        {
            /*
             * JS - 6 digit Julian Date
             * JY - 7 digit Julian Date
             * JL - 11 digit Julian Date
             */
            DateTime? parseResult = null;

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(format) && !value.Equals(" "))
            {
                // Ignore the '.' in the date
                int dotIndex = value.IndexOf('.');
                if (dotIndex != -1)
                {
                    value = value.Substring(0, dotIndex);
                }

                if (format.StartsWith("JS", StringComparison.CurrentCulture))
                {
                    if (value.Length == 6)
                    {
                        int year = -1, days = -1;
                        if (int.TryParse(value.Substring(0, 3), out year) && int.TryParse(value.Substring(3, 3), out days))
                        {
                            year += 1900;
                            DateTime date = new DateTime(year, 1, 1);
                            date = date.AddDays(((int)days) - 1);
                            parseResult = date;
                        }
                    }

                    return parseResult;
                }
                else if (format.StartsWith("JY", StringComparison.CurrentCulture) && value.Length == 7)
                {
                    int year = -1, days = -1;
                    if (int.TryParse(value.Substring(0, 4), out year) && int.TryParse(value.Substring(4, 3), out days))
                    {
                        DateTime date = new DateTime(year, 1, 1);
                        date = date.AddDays(((int)days) - 1);
                        parseResult = date;
                    }

                    return parseResult;
                }
                else if (format.StartsWith("JL", StringComparison.CurrentCulture) && value.Length == 11)
                {
                    format = "MMddyyyy";
                    value = value.Substring(0, 8);
                }
                else if (value.Length == 7)
                {
                    /* 7061997 (specific case MMDDYYYY, where the month is not of 2 digits) : prefixing a zero. */
                    value = string.Concat("0", value);
                }

                DateTime result = DateTime.MinValue;

                /*
                 TODO: Davins Add, comment on the logic of why we are doing 2 cultures....
                 */
                if (DateTime.TryParseExact(value, format, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out result))
                {
                    parseResult = result;
                }

                if (parseResult == null)
                {
                    if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out result))
                    {
                        parseResult = result;
                    }
                }
            }

            return parseResult;
        }

        public static string FormatNumeric(double? value)
        {
            string formattedValue = string.Empty;
            if (value.HasValue)
            {
                formattedValue = string.Format(CultureInfo.CreateSpecificCulture("id-ID"), "{0:N}", value);
            }
            return formattedValue;
        }

        public static double? GetParsedDouble(string value, bool useCurrentCultureSetting)
        {
            double parsedValue = 0;

            if (!string.IsNullOrEmpty(value))
            {
                CultureInfo culture = new CultureInfo("en-US");

                if (useCurrentCultureSetting)
                {
                    culture = CultureInfo.CurrentCulture;
                }

                if (double.TryParse(value, NumberStyles.Any, culture, out parsedValue))
                {
                    return parsedValue;
                }
            }

            return null;
        }

        public static string ToStringExact(DateTime? value, string format)
        {
            string formattedValue = string.Empty;
            if (value.HasValue)
            {
                formattedValue = value.Value.ToString(format, CultureInfo.CurrentCulture);
            }
            return formattedValue;
        }

        public static long? GetParsedNumericLong(string value, bool useCurrentCultureSetting)
        {
            long parsedValue = 0;
            if (!string.IsNullOrEmpty(value))
            {
                CultureInfo culture = new CultureInfo("en-US");
                if (useCurrentCultureSetting)
                {
                    culture = CultureInfo.CurrentCulture;
                }
                if (long.TryParse(value, NumberStyles.Any, culture, out parsedValue))
                {
                    return parsedValue;
                }
            }
            return null;
        }
    }
}