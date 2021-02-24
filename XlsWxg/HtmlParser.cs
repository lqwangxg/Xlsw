﻿using ExcelDna.Integration;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace XlsWxg
{
    public class HtmlParser
    {
        public static string HistoryFile
        {
            get
            {
                return Config.GetAppSettingValue2("history.file", "history.txt");
            }
        }

        [ExcelFunction(Category = "String", Description = "Get Match Group string")]
        public static string GetInnerText(string htmlpath, string findString)
        {
            Dictionary<string, HtmlNodeCollection> lstNode = GetNodes(htmlpath, findString);

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, HtmlNodeCollection> kv in lstNode)
            {
                foreach (var n in kv.Value)
                {
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
        [ExcelFunction(Category = "String", Description = "Get Match Group string")]
        public static string GetOutHtml(string htmlpath, string findString)
        {
            Dictionary<string, HtmlNodeCollection> lstNode = GetNodes(htmlpath, findString);

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, HtmlNodeCollection> kv in lstNode)
            {
                foreach (var n in kv.Value)
                {
                    if (n.Attributes.Contains("type"))
                    {
                        sb.Append("type:").Append(n.Attributes["type"]).Append(",");
                    }
                    sb.Append("html:").Append(n.OuterHtml);
                    sb.AppendLine();
                }
            }

            return WebUtility.HtmlDecode(sb.ToString());
        }
        public static Dictionary<string, HtmlNodeCollection> GetNodes(string htmlpath, string findString)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(htmlpath, Config.Encoding);

            string[] finds = Regex.Split(findString, @";|\n");
            Dictionary<string, HtmlNodeCollection> lstNode = new Dictionary<string, HtmlNodeCollection>();
            foreach (var key in finds)
            {
                if (string.IsNullOrEmpty(key)) continue;

                var ns = htmlDoc.DocumentNode.SelectNodes(key);
                if (ns == null) continue;
                lstNode.Add(key, ns);
            }

            return lstNode;
        }
        public static string GetAttText(string htmlpath, string findString)
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
