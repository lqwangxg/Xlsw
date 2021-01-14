using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnHtmlParser_Click(object sender, EventArgs e)
        {
            FormHtmlParser form = new FormHtmlParser();
            form.Show();
        }

        private void btnIEWebDriver_Click(object sender, EventArgs e)
        {
            FormIEWebDriver form = new FormIEWebDriver();
            form.Show();
        }
    }
}
