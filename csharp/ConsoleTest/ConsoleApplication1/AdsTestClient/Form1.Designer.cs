namespace AdsTestClient
{
    partial class AdsTestClient
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
            this.txt_url = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txt_response = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(24, 23);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(802, 20);
            this.txt_url.TabIndex = 0;
            this.txt_url.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 334);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 50);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_response
            // 
            this.txt_response.Location = new System.Drawing.Point(24, 67);
            this.txt_response.Multiline = true;
            this.txt_response.Name = "txt_response";
            this.txt_response.Size = new System.Drawing.Size(802, 261);
            this.txt_response.TabIndex = 3;
            // 
            // AdsTestClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 396);
            this.Controls.Add(this.txt_response);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_url);
            this.Name = "AdsTestClient";
            this.Text = "AdsTestClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_response;
    }
}

