using LongkeyMusic.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace LongkeyMusic
{
	public class DeleteConfirmBox : Form
	{
		private static DeleteConfirmBox _instance;
		private IContainer components;
		private PictureBox pictureBox1;
		private Label label1;
		private Button confirm;
		private Button cancel;
		public CheckBox deleteFileCheckBox;
		public static DeleteConfirmBox Instance
		{
			get
			{
				if (DeleteConfirmBox._instance == null || DeleteConfirmBox._instance.IsDisposed)
				{
					DeleteConfirmBox._instance = new DeleteConfirmBox();
				}
				return DeleteConfirmBox._instance;
			}
		}
		private DeleteConfirmBox()
		{
			this.InitializeComponent();
		}
		private void confirm_Click(object sender, EventArgs e)
		{
			if (this.deleteFileCheckBox.Checked)
			{
				base.DialogResult = DialogResult.Yes;
				return;
			}
			base.DialogResult = DialogResult.OK;
		}
		private void DeleteConfirmBox_FormClosed(object sender, FormClosedEventArgs e)
		{
			DeleteConfirmBox._instance = null;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DeleteConfirmBox));
			this.label1 = new Label();
			this.deleteFileCheckBox = new CheckBox();
			this.confirm = new Button();
			this.cancel = new Button();
			this.pictureBox1 = new PictureBox();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(141, 51);
			this.label1.Margin = new Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(232, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "删除提示：你确定要删除任务吗？";
			this.deleteFileCheckBox.AutoSize = true;
			this.deleteFileCheckBox.Location = new Point(15, 120);
			this.deleteFileCheckBox.Margin = new Padding(4, 4, 4, 4);
			this.deleteFileCheckBox.Name = "deleteFileCheckBox";
			this.deleteFileCheckBox.Size = new Size(179, 19);
			this.deleteFileCheckBox.TabIndex = 2;
			this.deleteFileCheckBox.Text = "同时删除磁盘上的文件";
			this.deleteFileCheckBox.UseVisualStyleBackColor = true;
			this.confirm.Location = new Point(261, 111);
			this.confirm.Margin = new Padding(4, 4, 4, 4);
			this.confirm.Name = "confirm";
			this.confirm.Size = new Size(100, 29);
			this.confirm.TabIndex = 1;
			this.confirm.Text = "确定";
			this.confirm.UseVisualStyleBackColor = true;
			this.confirm.Click += new EventHandler(this.confirm_Click);
			this.cancel.DialogResult = DialogResult.Cancel;
			this.cancel.Location = new Point(383, 111);
			this.cancel.Margin = new Padding(4, 4, 4, 4);
			this.cancel.Name = "cancel";
			this.cancel.Size = new Size(100, 29);
			this.cancel.TabIndex = 0;
			this.cancel.Text = "取消";
			this.cancel.UseVisualStyleBackColor = true;
			this.pictureBox1.Image = Resources.warning_64;
			this.pictureBox1.Location = new Point(28, 15);
			this.pictureBox1.Margin = new Padding(4, 4, 4, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(85, 80);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new SizeF(8f, 15f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.cancel;
			base.ClientSize = new Size(512, 159);
			base.Controls.Add(this.cancel);
			base.Controls.Add(this.confirm);
			base.Controls.Add(this.deleteFileCheckBox);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.pictureBox1);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Margin = new Padding(4, 4, 4, 4);
			base.MaximizeBox = false;
			this.MaximumSize = new Size(518, 192);
			base.MinimizeBox = false;
			this.MinimumSize = new Size(518, 192);
			base.Name = "DeleteConfirmBox";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "删除任务";
			base.TopMost = true;
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
