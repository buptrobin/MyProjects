using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;


namespace ngwallpaper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string beginUrl = "http://photography.nationalgeographic.com/photography/photo-of-the-day/";

            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            progressBar1.Value = 10;

            //get impage page url
            Stream data = client.OpenRead(beginUrl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();

            int pos1;
            int pos2;
            int len;
            //get picture url
            pos1 = s.IndexOf("primary_photo");
            pos2 = s.IndexOf("<img src=", pos1);
            int pos3 = s.IndexOf("width=", pos2);
            len = pos3 - 3 - (pos2 + 10);
            string fileurl = s.Substring(pos2 + 10, len);
            /*
            pos1 = s.IndexOf("download_link");
            pos2 = s.IndexOf("Download Wallpaper", pos1);
            len = pos2 - (pos1 + 24) - 2;
            string fileurl = s.Substring(pos1 + 24, len);

             * */

            outputText.Text = fileurl;
            progressBar1.Value = 50;

            Wallpaper.set(new Uri(fileurl), Wallpaper.Style.Stretched);
            progressBar1.Value = 100;
            
            
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
