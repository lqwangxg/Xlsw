using ExcelDna.Integration;
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

        [ExcelFunction(Category = "String", Description = "Replace string")]
        public static string ReplaceMatch(string input, string pattern, string replacement)
        {
            return ReplaceMatchGroup(input, pattern, 0, replacement);
        }

        [ExcelFunction(Category = "String", Description = "Replace string")]
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

        private static string ReplaceCC(Match m, int groupIndex, string replacement)
        {
            return m.Value.Replace(m.Groups[groupIndex].Value, replacement);
        }
    }
}
