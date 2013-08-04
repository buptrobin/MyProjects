using DevComponents.DotNetBar;
using LongkeyMusic.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
namespace LongkeyMusic
{
	public class TaskForm : Form
	{
		private string _historyPathsFile = Util.GetLocalDir() + "\\paths.data";
		private List<string> _pathList = new List<string>();
		private static TaskForm _instance;
		private IContainer components;
		private Button btnConfirmAddTask;
		private Button btnCancelTask;
		public TextBox textboxAlbumUrl;
		private FolderBrowserDialog baseFolderBrowser;
		private ComboBox baseFolderDropdown;
		private ButtonX chooseFolder;
		public static TaskForm Instance
		{
			get
			{
				if (TaskForm._instance == null || TaskForm._instance.IsDisposed)
				{
					TaskForm._instance = new TaskForm();
					TaskForm._instance.LoadHistoryFolderList();
				}
				return TaskForm._instance;
			}
		}
		private TaskForm()
		{
			this.InitializeComponent();
		}
		private void Serialize()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(this._historyPathsFile, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, this._pathList);
			stream.Close();
		}
		public void LoadHistoryFolderList()
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			try
			{
				Stream stream = new FileStream(this._historyPathsFile, FileMode.Open, FileAccess.Read, FileShare.Read);
				this._pathList = (List<string>)binaryFormatter.Deserialize(stream);
				stream.Close();
				foreach (string current in this._pathList)
				{
					this.baseFolderDropdown.Items.Add(current);
				}
				if (this._pathList.Count > 0)
				{
					this.baseFolderDropdown.Text = this._pathList[0];
				}
			}
			catch (Exception)
			{
				this._pathList = new List<string>();
			}
		}
		private void btnConfirmAddTask_Click(object sender, EventArgs e)
		{
			char[] separator = Environment.NewLine.ToCharArray();
			string[] array = this.textboxAlbumUrl.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (Util.IsValidAlbumUrl(text) == Util.AlbumType.Unrecognize)
				{
					MessageBox.Show("无法解释URL：" + text);
					return;
				}
			}
			string text2 = this.baseFolderDropdown.Text;
			try
			{
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("目录名\"" + this.baseFolderDropdown.Text + "\"不合法\n" + ex.Message);
				return;
			}
			if (!this._pathList.Contains(text2))
			{
				this.baseFolderDropdown.Items.Insert(0, text2);
			}
			else
			{
				this._pathList.Remove(text2);
			}
			this._pathList.Insert(0, text2);
			this.Serialize();
			string[] array3 = array;
			for (int j = 0; j < array3.Length; j++)
			{
				string text3 = array3[j];
				if (DataAndUIMaster.Instance.GetAlbum(text3) == null)
				{
					AlbumMeta albumMeta = AlbumFactory.Instance.CreateNewAlbum(text3);
					albumMeta.BaseFolder = text2;
					albumMeta.Refresh();
				}
				else
				{
					MessageBox.Show(text3 + " 该专辑已经在下载列表中");
				}
			}
			this.textboxAlbumUrl.Text = "";
			base.Close();
		}
		private void btnCancelTask_Click(object sender, EventArgs e)
		{
			this.textboxAlbumUrl.Text = "";
			base.Close();
		}
		private void TaskForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			TaskForm._instance = null;
		}
		private void chooseFolder_Click(object sender, EventArgs e)
		{
			if (this.baseFolderBrowser.ShowDialog() == DialogResult.OK)
			{
				string selectedPath = this.baseFolderBrowser.SelectedPath;
				this.baseFolderDropdown.Text = selectedPath;
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TaskForm));
			this.textboxAlbumUrl = new TextBox();
			this.btnConfirmAddTask = new Button();
			this.btnCancelTask = new Button();
			this.baseFolderBrowser = new FolderBrowserDialog();
			this.baseFolderDropdown = new ComboBox();
			this.chooseFolder = new ButtonX();
			base.SuspendLayout();
			this.textboxAlbumUrl.BorderStyle = BorderStyle.None;
			this.textboxAlbumUrl.Location = new Point(3, 0);
			this.textboxAlbumUrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.textboxAlbumUrl.Multiline = true;
			this.textboxAlbumUrl.Name = "textboxAlbumUrl";
			this.textboxAlbumUrl.Size = new Size(604, 208);
			this.textboxAlbumUrl.TabIndex = 0;
			this.btnConfirmAddTask.Location = new Point(460, 213);
			this.btnConfirmAddTask.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnConfirmAddTask.Name = "btnConfirmAddTask";
			this.btnConfirmAddTask.Size = new Size(74, 25);
			this.btnConfirmAddTask.TabIndex = 1;
			this.btnConfirmAddTask.Text = "下载";
			this.btnConfirmAddTask.UseVisualStyleBackColor = true;
			this.btnConfirmAddTask.Click += new EventHandler(this.btnConfirmAddTask_Click);
			this.btnCancelTask.DialogResult = DialogResult.Cancel;
			this.btnCancelTask.Location = new Point(542, 213);
			this.btnCancelTask.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnCancelTask.Name = "btnCancelTask";
			this.btnCancelTask.Size = new Size(65, 25);
			this.btnCancelTask.TabIndex = 2;
			this.btnCancelTask.Text = "取消";
			this.btnCancelTask.UseVisualStyleBackColor = true;
			this.btnCancelTask.Click += new EventHandler(this.btnCancelTask_Click);
			this.baseFolderDropdown.FormattingEnabled = true;
			this.baseFolderDropdown.Location = new Point(3, 213);
			this.baseFolderDropdown.Margin = new System.Windows.Forms.Padding(0);
			this.baseFolderDropdown.MaxDropDownItems = 10;
			this.baseFolderDropdown.Name = "baseFolderDropdown";
			this.baseFolderDropdown.Size = new Size(404, 25);
			this.baseFolderDropdown.TabIndex = 4;
			this.chooseFolder.AccessibleRole = AccessibleRole.PushButton;
			this.chooseFolder.ColorTable = eButtonColor.OrangeWithBackground;
			this.chooseFolder.Image = Resources.choose_folder;
			this.chooseFolder.Location = new Point(417, 213);
			this.chooseFolder.Margin = new System.Windows.Forms.Padding(0);
			this.chooseFolder.Name = "chooseFolder";
			this.chooseFolder.Size = new Size(28, 25);
			this.chooseFolder.Style = eDotNetBarStyle.StyleManagerControlled;
			this.chooseFolder.TabIndex = 5;
			this.chooseFolder.Click += new EventHandler(this.chooseFolder_Click);
			base.AcceptButton = this.btnConfirmAddTask;
			base.AutoScaleDimensions = new SizeF(7f, 17f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancelTask;
			base.ClientSize = new Size(611, 241);
			base.ControlBox = false;
			base.Controls.Add(this.chooseFolder);
			base.Controls.Add(this.baseFolderDropdown);
			base.Controls.Add(this.btnCancelTask);
			base.Controls.Add(this.btnConfirmAddTask);
			base.Controls.Add(this.textboxAlbumUrl);
			this.DoubleBuffered = true;
			this.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TaskForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "新建任务";
			base.TopMost = true;
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
