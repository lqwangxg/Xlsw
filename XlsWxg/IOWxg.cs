using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

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
                    string fileName = UtilWxg.ReplaceMatchGroup(fi.Name, pattern, groupIndex, replacement);
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
                string folderName = UtilWxg.ReplaceMatchGroup(di.Name, pattern, groupIndex, replacement);
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
                string replaced = UtilWxg.ReplaceMatchGroup(input, pattern, groupIndex, replacement);
                if(!input.Equals(replaced))
                {
                    isChanged = true;
                    fi.MoveTo(fi.FullName + DateTime.Now.ToString("_yyyyMMddhhmmss.bak"));
                    File.WriteAllText(oldFileName, replaced);
                }
            }
            return isChanged;
        }
       


        #endregion
    }
}
