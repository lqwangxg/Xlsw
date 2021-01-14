
namespace TestForm
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnHtmlParser = new System.Windows.Forms.Button();
            this.btnIEWebDriver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHtmlParser
            // 
            this.btnHtmlParser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHtmlParser.Location = new System.Drawing.Point(55, 51);
            this.btnHtmlParser.Name = "btnHtmlParser";
            this.btnHtmlParser.Size = new System.Drawing.Size(149, 39);
            this.btnHtmlParser.TabIndex = 1;
            this.btnHtmlParser.Text = "HtmlParser";
            this.btnHtmlParser.UseVisualStyleBackColor = true;
            this.btnHtmlParser.Click += new System.EventHandler(this.btnHtmlParser_Click);
            // 
            // btnIEWebDriver
            // 
            this.btnIEWebDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIEWebDriver.Location = new System.Drawing.Point(233, 51);
            this.btnIEWebDriver.Name = "btnIEWebDriver";
            this.btnIEWebDriver.Size = new System.Drawing.Size(149, 39);
            this.btnIEWebDriver.TabIndex = 1;
            this.btnIEWebDriver.Text = "IEWebDriver";
            this.btnIEWebDriver.UseVisualStyleBackColor = true;
            this.btnIEWebDriver.Click += new System.EventHandler(this.btnIEWebDriver_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnIEWebDriver);
            this.Controls.Add(this.btnHtmlParser);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHtmlParser;
        private System.Windows.Forms.Button btnIEWebDriver;
    }
}