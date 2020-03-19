using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XlsWxg
{
    public class IOWxg
    {

        [ExcelFunction(Category = "String", Description = "Log string to currentDiretory.")]
        public static void AppendTextFile(string path, string content)
        {
            File.AppendAllText(path, content, Encoding.UTF8);
        }

        [ExcelFunction(Category = "File", Description = "Replace string in text file under directory")]

        public static string ChangeConnectionString(string strPath, string newConnectionString)
        {
            string searchPattern = @"*.config";
            string pattern = @"alias=""defaultDB""\s+descriptor=""([^""]+)""";

            return ReplaceFiles(strPath, searchPattern, pattern, 1, newConnectionString);
        }
     
        [ExcelFunction(Category = "File", Description = "Replace string in text file under directory")]
        public static string ReplaceFiles(string diretoryName, string searchPattern, string pattern, int groupIndex, string replacement)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("method=").AppendLine("ReplaceFiles");
            sb.Append("diretoryName=").AppendLine(diretoryName);
            sb.Append("searchPattern=").AppendLine(searchPattern);
            sb.Append("pattern=").AppendLine(pattern);
            sb.Append("groupIndex=").AppendLine(groupIndex.ToString());
            sb.Append("replacement=").AppendLine(replacement);
            Log(sb.ToString());

            List<string> lstFile = new List<string>();
            DirectoryInfo di = new DirectoryInfo(diretoryName);
            if (di.Exists)
            {
                foreach (FileInfo fi in di.GetFiles(searchPattern))
                {
                    //ファイル名変更
                    string fileName = ReplaceRegex(fi.Name, pattern, groupIndex, replacement);
                    if (!fileName.Equals(fi.Name))
                    {
                        string newFileName = fi.FullName.Replace(fi.Name, fileName);
                        fi.MoveTo(newFileName);
                    }
                    //ファイル内容変更
                    if (ReplaceFile(fi.FullName, searchPattern, pattern, groupIndex, replacement))
                    {
                        lstFile.Add(fi.FullName);
                    }
                }
                //フォルダ名変更
                string folderName = ReplaceRegex(di.Name, pattern, groupIndex, replacement);
                if (!folderName.Equals(di.Name))
                {
                    string newFolderName = di.FullName.Replace(di.Name, folderName);
                    di.MoveTo(newFolderName);
                }
                //サブフォルダ内ファイルの変更
                foreach (DirectoryInfo fi in di.GetDirectories())
                {
                    lstFile.Add(ReplaceFiles(fi.FullName, searchPattern, pattern, groupIndex, replacement) + Environment.NewLine);
                }
            }
            return string.Join(Environment.NewLine, lstFile);
        }

        [ExcelFunction(Category = "File", Description = "Replace string in text file under directory")]
        public static bool ReplaceFile(string filePath, string searchPattern, string pattern, int groupIndex, string replacement)
        {
            return ReplaceFile(new FileInfo(filePath), searchPattern, pattern, groupIndex, replacement);
        }
        
        [ExcelFunction(Category = "String", Description = "Replace string")]
        public static string ReplaceRegex(string input, string pattern, int groupIndex, string replacement)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("method=").AppendLine("ReplaceRegex");
            sb.Append("input=").AppendLine(input);
            sb.Append("pattern=").AppendLine(pattern);
            sb.Append("groupIndex=").AppendLine(groupIndex.ToString());
            sb.Append("replacement=").AppendLine(replacement);

            Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (regex.IsMatch(input))
            {
                sb.Append("IsMatch=").AppendLine("OK");
                Log(sb.ToString());
                return regex.Replace(input, new MatchEvaluator(
                    delegate (Match match)
                    {
                        return ReplaceCC(match, groupIndex, replacement);
                    }));
            }
            else
            {
                sb.Append("IsMatch=").AppendLine("NG");
                Log(sb.ToString());
            }

            return input;
        }

        public static void Log(string content)
        {
            string isWriteLog = ConfigurationManager.AppSettings.Get("writeLog");
            bool writeLog = false;
            bool.TryParse(isWriteLog, out writeLog);
            if (!writeLog) return;
            
            string currPath = Environment.CurrentDirectory + "\\Application_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            File.AppendAllText(currPath, content, Encoding.UTF8);
        }

        #region private methods

        private static bool ReplaceFile(FileInfo fi, string searchPattern, string pattern, int groupIndex, string replacement)
        {
            bool isChanged = false;
            if (fi.Exists)
            {
                string oldFileName = fi.FullName;
                string input = File.ReadAllText(fi.FullName);
                string replaced = ReplaceRegex(input, pattern, groupIndex, replacement);
                if(!input.Equals(replaced))
                {
                    isChanged = true;
                    fi.MoveTo(fi.FullName + DateTime.Now.ToString("_yyyyMMddhhmmss.bak"));
                    File.WriteAllText(oldFileName, replaced);
                }
            }
            return isChanged;
        }
                
        private static string ReplaceCC(Match m, int groupIndex, string replacement)
        {
            if (m.Groups.Count == groupIndex) 
            {
                groupIndex--; 
            }
            return m.Value.Replace(m.Groups[groupIndex].Value, replacement);
        }


        #endregion
    }
}
