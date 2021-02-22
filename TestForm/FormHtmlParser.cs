using System;

using System.IO;
using System.Windows.Forms;
using XlsWxg;
namespace TestForm
{
    public partial class FormHtmlParser : Form
    {
        public FormHtmlParser()
        {
            InitializeComponent();
            LoadHistory();
        }
        private void LoadHistory()
        {
            if (!File.Exists(HtmlParser.HistoryFile)) return;
            string[] lines = File.ReadAllLines(HtmlParser.HistoryFile);
            foreach(string line in lines)
            {
                listHistory.Items.Add(line);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text)) return;

            try
            {
                txtResult.Text = HtmlParser.GetInnerText(txtFilePath.Text, txtFilter.Text);
                File.AppendAllText(HtmlParser.HistoryFile, txtFilter.Text + @"\n", Config.Encoding);
                listHistory.Items.Add(txtFilter.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n"+ ex.StackTrace,"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listHistory_DoubleClick(object sender, EventArgs e)
        {
            if (listHistory.SelectedIndex > -1)
            {
                txtResult.Text = listHistory.SelectedItem.ToString();
            }
        }
    }
}
