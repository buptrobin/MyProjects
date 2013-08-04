using DevComponents.DotNetBar;
using LongkeyMusic.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace LongkeyMusic
{
	public class DownloadFinishToolTipForm : Form
	{
		public const int AW_HOR_POSITIVE = 1;
		public const int AW_HOR_NEGATIVE = 2;
		public const int AW_VER_POSITIVE = 4;
		public const int AW_VER_NEGATIVE = 8;
		public const int AW_CENTER = 16;
		public const int AW_HIDE = 65536;
		public const int AW_ACTIVATE = 131072;
		public const int AW_SLIDE = 262144;
		public const int AW_BLEND = 524288;
		private Timer Timer_Close;
		private static DownloadFinishToolTipForm _instance;
		private string _albumPath;
		private IContainer components;
		private PictureBox pictureBox1;
		private LabelX labelAlbum;
		private LabelX labelArtist;
		private LabelX labelX1;
		private ButtonX closeButton;
		public static DownloadFinishToolTipForm Instance
		{
			get
			{
				if (DownloadFinishToolTipForm._instance == null || DownloadFinishToolTipForm._instance.IsDisposed)
				{
					DownloadFinishToolTipForm._instance = new DownloadFinishToolTipForm();
				}
				return DownloadFinishToolTipForm._instance;
			}
		}
		[DllImport("user32.dll")]
		private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
		private void DownloadFinishToolTipForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			DownloadFinishToolTipForm._instance = null;
			DownloadFinishToolTipForm.AnimateWindow(base.Handle, 500, 327696);
		}
		private void Timer_Close_Tick(object sender, EventArgs e)
		{
			this.Timer_Close.Stop();
			base.Close();
		}
		private void DownloadFinishToolTipForm_MouseMove(object sender, MouseEventArgs e)
		{
			this.Timer_Close.Stop();
		}
		private void DownloadFinishToolTipForm_MouseLeave(object sender, EventArgs e)
		{
			this.Timer_Close.Start();
		}
		private void DownloadFinishToolTipForm_Load(object sender, EventArgs e)
		{
			int width = Screen.PrimaryScreen.Bounds.Width;
			int height = Screen.PrimaryScreen.Bounds.Height;
			width = Screen.PrimaryScreen.WorkingArea.Width;
			height = Screen.PrimaryScreen.WorkingArea.Height;
			int top = height - base.Height;
			int left = width - base.Width;
			base.Top = top;
			base.Left = left;
			base.TopMost = true;
			DownloadFinishToolTipForm.AnimateWindow(base.Handle, 800, 262152);
			this.Timer_Close.Start();
		}
		private DownloadFinishToolTipForm()
		{
			this.InitializeComponent();
			this.Timer_Close = new Timer();
			this.Timer_Close.Interval = 5000;
			this.Timer_Close.Tick += new EventHandler(this.Timer_Close_Tick);
		}
		public void SetForm(AlbumMeta album)
		{
			if (this.pictureBox1.InvokeRequired)
			{
				SetImageHandler method = delegate(Image img)
				{
					this.pictureBox1.Image = img;
				};
				base.Invoke(method, new object[]
				{
					album.CoverThumnail
				});
			}
			else
			{
				this.pictureBox1.Image = album.CoverThumnail;
			}
			if (this.labelAlbum.InvokeRequired)
			{
				SetMessageHandler method2 = delegate(string msg)
				{
					this.labelAlbum.Text = msg;
				};
				base.Invoke(method2, new object[]
				{
					album.Album
				});
			}
			else
			{
				this.labelAlbum.Text = album.Album;
			}
			if (this.labelArtist.InvokeRequired)
			{
				SetMessageHandler method3 = delegate(string msg)
				{
					this.labelArtist.Text = msg;
				};
				base.Invoke(method3, new object[]
				{
					album.Artist
				});
			}
			else
			{
				this.labelArtist.Text = album.Artist;
			}
			this._albumPath = Util.GetAlbumDir(album);
		}
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(this._albumPath);
			}
			catch (Exception)
			{
			}
		}
		private void closeButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.labelAlbum = new LabelX();
			this.labelArtist = new LabelX();
			this.labelX1 = new LabelX();
			this.closeButton = new ButtonX();
			this.pictureBox1 = new PictureBox();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.labelAlbum.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelAlbum.Location = new Point(124, 22);
			this.labelAlbum.Name = "labelAlbum";
			this.labelAlbum.Size = new Size(75, 23);
			this.labelAlbum.TabIndex = 1;
			this.labelAlbum.Text = "album";
			this.labelArtist.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelArtist.Location = new Point(124, 51);
			this.labelArtist.Name = "labelArtist";
			this.labelArtist.Size = new Size(75, 23);
			this.labelArtist.TabIndex = 2;
			this.labelArtist.Text = "artist";
			this.labelX1.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX1.Location = new Point(124, 80);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new Size(75, 23);
			this.labelX1.TabIndex = 3;
			this.labelX1.Text = "下载完成 !!";
			this.closeButton.AccessibleRole = AccessibleRole.PushButton;
			this.closeButton.ColorTable = eButtonColor.OrangeWithBackground;
			this.closeButton.Image = Resources.close_12;
			this.closeButton.Location = new Point(206, 6);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new Size(12, 12);
			this.closeButton.Style = eDotNetBarStyle.StyleManagerControlled;
			this.closeButton.TabIndex = 4;
			this.closeButton.Click += new EventHandler(this.closeButton_Click);
			this.pictureBox1.Cursor = Cursors.Hand;
			this.pictureBox1.Location = new Point(9, 6);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(100, 100);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.GradientActiveCaption;
			base.ClientSize = new Size(230, 114);
			base.Controls.Add(this.closeButton);
			base.Controls.Add(this.labelX1);
			base.Controls.Add(this.labelArtist);
			base.Controls.Add(this.labelAlbum);
			base.Controls.Add(this.pictureBox1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DownloadFinishToolTipForm";
			base.StartPosition = FormStartPosition.Manual;
			this.Text = "DownloadFinishToolTipForm";
			base.Load += new EventHandler(this.DownloadFinishToolTipForm_Load);
			base.FormClosed += new FormClosedEventHandler(this.DownloadFinishToolTipForm_FormClosed);
			base.MouseLeave += new EventHandler(this.DownloadFinishToolTipForm_MouseLeave);
			base.MouseMove += new MouseEventHandler(this.DownloadFinishToolTipForm_MouseMove);
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
