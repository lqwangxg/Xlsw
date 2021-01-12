using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace XlsWxg
{
    public class UtilWxg
    {

        [ExcelFunction(Category = "String", Description = "Replace string")]
        public static string ReplaceMatchGroup(string input, string pattern, int groupIndex, string replacement)
        {
            Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (regex.IsMatch(input))
            {
                return regex.Replace(input, new MatchEvaluator(
                    delegate (Match match)
                    {
                        return ReplaceCC(match, groupIndex, replacement);
                    }));
            }
            return input;
        }

        [ExcelFunction(Category = "String", Description = "Replace replacement on found pattern")]
        public static string ReplaceMatch(string input, string pattern, string replacement)
        {
            return ReplaceMatchGroup(input, pattern, 0, replacement);
        }

        [ExcelFunction(Category = "String", Description = "Replace string on found {key} or [key] or (:key)")]
        public static string ReplaceKeyValue(string input, string key, string value)
        {
            string pattern = "\\{\\s*" + key + "\\s*\\}";
            input = ReplaceMatchGroup(input, pattern, 0, value);

            pattern = "\\[\\s*" + key + "\\s*\\]";
            input = ReplaceMatchGroup(input, pattern, 0, value);
            
            pattern = "(:\\s*" + key + ")\\W";
            input = ReplaceMatchGroup(input, pattern, 1, value);

            return input;
        }
        [ExcelFunction(Category = "String", Description = "Get Match Group string")]
        public static string GetMatchGroup(string input, string pattern, int groupIndex) 
        {
            Match m = Regex.Match(input, pattern);
            if (m.Success) 
            {
                if (groupIndex < m.Groups.Count) 
                {
                    return m.Groups[groupIndex].Value;
                }
            }
            return string.Empty; 
        }

        private static string ReplaceCC(Match m, int groupIndex, string replacement)
        {
            return m.Value.Replace(m.Groups[groupIndex].Value, replacement);
        }

        public static string DataTabletoString(DataTable dt)
        {
            string header = string.Join("|", dt.Columns.OfType<DataColumn>().Select(x => x.ColumnName));
            List<string> lstTable = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                List<string> lstRow = new List<string>();
                lstRow.Clear();
                foreach (DataColumn col in dt.Columns)
                {
                    if (!row.IsNull(col))
                    {
                        lstRow.Add(row[col].ToString());
                    }
                    else
                    {
                        lstRow.Add(string.Empty);
                    }
                }
                lstTable.Add(string.Join("|", lstRow));
            }
            string datas = string.Join(Environment.NewLine, lstTable);
            return header + Environment.NewLine + datas;
        }
    }
}
