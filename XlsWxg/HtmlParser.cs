using ExcelDna.Integration;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;


namespace XlsWxg
{
    public class HtmlParser
    {
        public static Encoding thisEncoding
        {
            get
            {
                string encoding = ConfigurationManager.AppSettings.Get("encoding");
                if (string.IsNullOrEmpty(encoding))
                {
                    return Encoding.UTF8;
                }
                else
                {
                    return Encoding.GetEncoding(encoding);
                }
            }
        }
        [ExcelFunction(Category = "String", Description = "Get Match Group string")]
        public static string GetInnerText(string htmlpath, string findString)
        {
            
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(htmlpath, thisEncoding);

            string[] finds = findString.Split(";".ToCharArray());
            Dictionary<string,  HtmlNodeCollection> lstNode = new Dictionary<string, HtmlNodeCollection>();
            foreach (var key in finds) 
            {
                var ns = htmlDoc.DocumentNode.SelectNodes(key);
                if (ns == null) continue;
                lstNode.Add(key, ns);
            }

            StringBuilder sb = new StringBuilder();
            string tagName = string.Empty;
            foreach (KeyValuePair<string, HtmlNodeCollection> kv in lstNode)
            {
                tagName = UtilWxg.GetMatchGroup(kv.Key, @"\/\/(\w+)", 1);
                sb.Append("##").Append(kv.Key).AppendLine();
                foreach (var n in kv.Value)
                {
                    sb.Append("tag:").Append(tagName).Append(",");
                    foreach (var atr in n.Attributes)
                    {
                        sb.Append(atr.Name).Append(":").Append(atr.Value).Append(",");
                    }
                    sb.Append("InnerText").Append(":").Append(n.InnerText);
                    sb.AppendLine();
                }
            }
            
            
            return WebUtility.HtmlDecode(sb.ToString());
        }
        private static string GetAttText(string htmlpath, string findString)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(htmlpath);
            var nodes = htmlDoc.DocumentNode.SelectNodes(findString);
            if (nodes == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (var n in nodes)
            {
                foreach (var atr in n.Attributes)
                {
                    sb.Append(atr.Name).Append(":").Append(atr.Value).Append(",");
                }
                sb.Append("InnerText").Append(":").Append(n.InnerText);
                sb.AppendLine();
            }

            return WebUtility.HtmlDecode(sb.ToString());
        }
    }
}
