using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Text;
using System.Reflection;

namespace CCARE.jqGrid
{
    public class Filters
    {
        public enum GroupOp
        {
            AND,
            OR
        }

        public enum Operations
        {
            eq, // "equal"
            ne, // "not equal"
            lt, // "less"
            le, // "less or equal"
            gt, // "greater"
            ge, // "greater or equal"
            bw, // "begins with"
            bn, // "does not begin with"
            //in, // "in"
            //ni, // "not in"
            ew, // "ends with"
            en, // "does not end with"
            cn, // "contains"
            nc  // "does not contain"
        }

        public class Rule
        {
            public string field { get; set; }
            public Operations op { get; set; }
            public string data { get; set; }
        }

        public GroupOp groupOp { get; set; }

        public List<Rule> rules { get; set; }

        private static readonly string[] FormatMapping = {
            "(it.{0} = @p{1})",                 // "eq" - equal
            "(it.{0} <> @p{1})",                // "ne" - not equal
            "(it.{0} < @p{1})",                 // "lt" - less than
            "(it.{0} <= @p{1})",                // "le" - less than or equal to
            "(it.{0} > @p{1})",                 // "gt" - greater than
            "(it.{0} >= @p{1})",                // "ge" - greater than or equal to
            "(it.{0} LIKE (@p{1}+'%'))",        // "bw" - begins with
            "(it.{0} NOT LIKE (@p{1}+'%'))",    // "bn" - does not begin with
            "(it.{0} LIKE ('%'+@p{1}))",        // "ew" - ends with
            "(it.{0} NOT LIKE ('%'+@p{1}))",    // "en" - does not end with
            "(it.{0} LIKE ('%'+@p{1}+'%'))",    // "cn" - contains
            "(it.{0} NOT LIKE ('%'+@p{1}+'%'))" //" nc" - does not contain
        };

        internal ObjectQuery<T> FilterObjectSet<T>(ObjectQuery<T> inputQuery) where T : class
        {
            if (rules.Count <= 0)
                return inputQuery;

            var sb = new StringBuilder();
            var objParams = new List<ObjectParameter>(rules.Count);

            foreach (Rule rule in rules)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(rule.field);
                if (propertyInfo == null)
                    continue; // skip wrong entries

                if (sb.Length != 0)
                    sb.Append(groupOp);

                var iParam = objParams.Count;
                sb.AppendFormat(FormatMapping[(int)rule.op], rule.field, iParam);

                ObjectParameter param;
                switch (propertyInfo.PropertyType.FullName)
                {
                    case "System.Int32":  // int
                        param = new ObjectParameter("p" + iParam, Int32.Parse(rule.data));
                        break;
                    case "System.Int64":  // bigint
                        param = new ObjectParameter("p" + iParam, Int64.Parse(rule.data));
                        break;
                    case "System.Int16":  // smallint
                        param = new ObjectParameter("p" + iParam, Int16.Parse(rule.data));
                        break;
                    case "System.SByte":  // tinyint
                        param = new ObjectParameter("p" + iParam, SByte.Parse(rule.data));
                        break;
                    case "System.Single": // Edm.Single, in SQL: float
                        param = new ObjectParameter("p" + iParam, Single.Parse(rule.data));
                        break;
                    case "System.Double": // float(53), double precision
                        param = new ObjectParameter("p" + iParam, Double.Parse(rule.data));
                        break;
                    case "System.Boolean": // Edm.Boolean, in SQL: bit
                        param = new ObjectParameter("p" + iParam,
                            String.Compare(rule.data, "1", StringComparison.Ordinal) == 0 ||
                            String.Compare(rule.data, "yes", StringComparison.OrdinalIgnoreCase) == 0 ||
                            String.Compare(rule.data, "true", StringComparison.OrdinalIgnoreCase) == 0 ?
                            true :
                            false);
                        break;
                    default:
                        // TODO: Extend to other data types
                        // binary, date, datetimeoffset,
                        // decimal, numeric,
                        // money, smallmoney
                        // and so on

                        param = new ObjectParameter("p" + iParam, rule.data);
                        break;
                }
                objParams.Add(param);
            }

            ObjectQuery<T> filteredQuery = inputQuery.Where(sb.ToString());
            foreach (var objParam in objParams)
                filteredQuery.Parameters.Add(objParam);

            return filteredQuery;
        }
    }
}