using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XlsWxg;
namespace TestForm
{
    public partial class FormHtmlParser : Form
    {
        public FormHtmlParser()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //string oracon = "User ID=proposal; Password=proposal; Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=192.16.108.26)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=BRONZEPDB)))";
            //string result = OraXE.Query("select sysdate from dual", oracon);
            //IOWxg.Log(result);

            txtResult.Text = HtmlParser.GetInnerText(txtFilePath.Text,txtFilter.Text);

        }
    }
}
