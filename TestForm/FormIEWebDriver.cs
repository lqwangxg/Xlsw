using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XlsWxg;
namespace TestForm
{
    public partial class FormIEWebDriver : Form
    {
        private const string URL = "https://www.google.co.jp/";
        private const string IE_DRIVER_PATH = @"IEDriverServer.exe";

        public FormIEWebDriver()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Test();
        }

        public static void Test()
        {
            IWebElement textbox;
            IWebElement findbuttom;
            var options = new InternetExplorerOptions()
            {
                InitialBrowserUrl = URL,
                IntroduceInstabilityByIgnoringProtectedModeSettings = true
            };
            //Webページを開く(String servicePath, Int32 port, String driverServiceExecutableName, Uri driverServiceDownloadUrl)
            IWebDriver driver = new InternetExplorerDriver(IE_DRIVER_PATH, options);
            driver.Navigate();

            //検索ボックス
            textbox = driver.FindElement(By.Name("q"));
            //検索ボックスに検索ワードを入力
            textbox.SendKeys("Selenium");

            //検索ボタン
            findbuttom = driver.FindElement(By.Name("btnK"));
            //検索ボタンをクリック
            findbuttom.Click();
            driver.Close(); // closes browser
            driver.Quit(); // closes IEDriverServer process

        }
    }
}
