using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.General;
using System.Globalization;

namespace CCARE.App_Function
{
    public class Utility
    {

        public static int CheckSpResult(int value)
        {
            if (value == 0 || value == 16 || value == 6)
            {
                return 0;
            }
            else return 1;
        }

        public static string GetDisplayText(string CategoryName, string ObjectName, string AttributeName, string Code)
        {
            CRMDb db = new CRMDb();

            var text = db.mapping
                .Where(x => x.CategoryName == CategoryName)
                .Where(x => x.ObjectName == ObjectName)
                .Where(x => x.AttributeName == AttributeName)
                .Where(x => x.Code == Code)
                .Where(x => x.StateCode == 0)
                .Select(x => x.Label);

            if (text.Count() > 0 && !string.IsNullOrEmpty(text.First()))
            {
                return text.First().ToString();
            }

            return Code;
        }

        public static string GetStringMap(int entityType, int statusType, string key)
        {
            CRMDb db = new CRMDb();

            var text = db.statusmapper
                .Where(x => x.EntityType == entityType)
                .Where(x => x.StatusType == statusType)
                .Where(x => x.Key == key)
                .Select(x => x.Value);

            if (text.Count() > 0 && !string.IsNullOrEmpty(text.First()))
            {
                return text.First().ToString();
            }
            return key;
        }

        public static string GetCustomerSegmentation(string customersegmentation, string type) {
            CRMDb db = new CRMDb();
            string custinfo = string.Empty;
            /*
             * 101 --> Segementation
             * 102 --> Membership
             */
            int statustype = "Segmentation".Equals(type) ? 101 : 102;
            var model = db.statusmapper
                .Where(x => x.EntityType == 19)
                .Where(x => x.StatusType == statustype)
                .Select(x => new
                {
                    k = x.Key,
                    v = x.Value
                });

            foreach (var item in model)
            {
                if (customersegmentation.Contains(item.k)) {
                    custinfo = item.v;
                    break;
                }
            }

            return custinfo;
        }

        public static string GetLabel(string entityName, string attributeName, int attributeValue)
        {
            CRMDb db = new CRMDb();

            var text = db.pickList
                .Where(r => r.EntityName == entityName)
                .Where(r => r.AttributeName == attributeName)
                .Where(r => r.AttributeValue == attributeValue)
                .Select(r => r.label);

            if (text.Count() > 0 && !string.IsNullOrEmpty(text.First()))
            {
                return text.First().ToString();
            }
            return String.Empty;
        }

        public static string GetChannelType(int attributeValue)
        {
            return GetLabel("channel", "channeltype", attributeValue);
        }

        public static string GetCategory(int attributeValue)
        {
            return GetLabel("channel", "category", attributeValue);
        }

        public static string GetAccountType(string code)
        {
            string value = Utility.GetDisplayText("AccountType", "Account", "AccountType", code);
            value = value != code ? value : code == String.Empty ? String.Empty : string.Format(CultureInfo.InvariantCulture, "{0} - Gagal", code);
            return value;
        }

        public static string GetLoanType(string code)
        {
            CRMDb db = new CRMDb();

            var text = db.mapping
                .Where(x => x.CategoryName == "LoanType")
                .Where(x => x.Code == code)
                .Where(x => x.StateCode == 0)
                .Select(x => x.Label);

            if (text.Count() > 0 && !string.IsNullOrEmpty(text.First()))
            {
                return text.First().ToString();
            }

            return code;
        }

        public static string GetCardTypeByCardNo(string cardNo)
        {
            CRMDb db = new CRMDb();

            var text = db.mapping
                .Where(x => x.CategoryName == "AccountCardType")
                .Where(x => x.ObjectName == "Account")
                .Where(x => x.AttributeName == "CardType")
                .Where(x => x.Code.StartsWith(cardNo.Substring(0,8)))
                .Where(x => x.StateCode == 0)
                .Select(x => x.Label);

            if (text.Count() > 0 && !string.IsNullOrEmpty(text.First()))
            {
                return text.First().ToString();
            }

            return cardNo;
        }

        public static string GetMiddlewareTandemStatus(string CategoryName, string ObjectName, string AttributeName, string Code)
        {
            string value = Utility.GetDisplayText(CategoryName, ObjectName, AttributeName, Code);
            value = value != Code ? value : Code == String.Empty ? String.Empty : string.Format(CultureInfo.InvariantCulture, "{0} - Gagal", Code);
            return value;
        }

        public static string GetCurrency(string Code)
        {
            CRMDb db = new CRMDb();

            var text = db.mapping
                .Where(x => x.CategoryName == "CurrencyCode")
                .Where(x => x.ObjectName == "Currency")
                .Where(x => x.AttributeName == "CurrencyCode")
                .Where(x => x.Code == Code)
                .Select(x => x.Label);

            if (text.Count() > 0 && !string.IsNullOrEmpty(text.First()))
            {
                return text.First().ToString();
            }

            return Code;
        }

        public static string GetStatusInfo(string Code)
        {
            CRMDb db = new CRMDb();
            var text = db.mapping
                .Where(x => x.AttributeName == "Inquiry Status")
                .Where(x => x.Code == Code)
                .Select(x => x.Label);

            if (text.Count() > 0 && !string.IsNullOrEmpty(text.First()))
            {
                return text.First().ToString();
            }

            return Code;
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
                /* biar jalan dulu */
                else if (value.Length == 11)
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

        public static string[] HistoricalTransactionCodesForNegativeAmount
        {
            get
            {
                return new string[] { "9", "11", "13", "15", "17", "19", "20", "21", "22", "23", "31", "33", "39", "41", "43", "49", "50", "61", "66", "67", "69", "91" };
            }
        }


        public static List<Guid?> getBUChild(List<Guid?> BUUpdate, Guid? parentBU)
        {
            CRMDb db = new CRMDb();
            List<Guid?> buQuery = db.businessunit.Where(x => x.ParentBusinessUnitId == parentBU).Select(x => x.BusinessUnitId).ToList();

            foreach (Guid? currentBU in buQuery)
            {
                BUUpdate.Add(currentBU);
                getBUChild(BUUpdate, currentBU);
            }

            return BUUpdate;
        }

        public static string FormatDateAuth(string input)
        {
            DateTime d = DateTime.ParseExact(input, "ddMMyyyy", CultureInfo.InvariantCulture);
            return d.ToString("dd/MM/yyyy");
        }
    }
}