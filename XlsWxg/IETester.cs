using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsWxg
{
    public class IETester
    {
        private const string URL = "https://www.google.co.jp/";
        private const string IE_DRIVER_PATH = @"IEDriverServer.exe";

        [ExcelFunction(Category = "String", Description = "AppendTextFile")]

        public static void Test()
        {
            //string oracon = "User ID=proposal; Password=proposal; Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=192.16.108.26)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=BRONZEPDB)))";
            //string result = OraXE.Query("select sysdate from dual", oracon);
            ////IOWxg.Log(result);
            //IWebElement textbox;
            //IWebElement findbuttom;
            //var options = new InternetExplorerOptions()
            //{
            //    InitialBrowserUrl = URL,
            //    IntroduceInstabilityByIgnoringProtectedModeSettings = true
            //};
            ////Webページを開く(String servicePath, Int32 port, String driverServiceExecutableName, Uri driverServiceDownloadUrl)
            //IWebDriver driver = new InternetExplorerDriver(IE_DRIVER_PATH, options);
            //driver.Navigate();

            ////検索ボックス
            //textbox = driver.FindElement(By.Name("q"));
            ////検索ボックスに検索ワードを入力
            //textbox.SendKeys("Selenium");

            ////検索ボタン
            //findbuttom = driver.FindElement(By.Name("btnK"));
            ////検索ボタンをクリック
            //findbuttom.Click();
            //driver.Close(); // closes browser
            //driver.Quit(); // closes IEDriverServer process

        }
    }
}
