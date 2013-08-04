using BrightIdeasSoftware;
using DevComponents.DotNetBar;
using LongkeyMusic.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace LongkeyMusic
{
	public class LongkeyMusicForm : Form
	{
		private IntPtr nextClipboardViewer;
		private bool _isFormInitialized;
		private IContainer components;
		private FolderBrowserDialog folderbrowserdialogBasePath;
		private NotifyIcon notifyIcon;
		private PictureBox pictureboxCoverPicture;
		private SuperTabItem superTabItem1;
		private SuperTabControl superTabControl1;
		private SuperTabControlPanel superTabControlPanel1;
		private SuperTabItem superTabItemDownloadingTab;
		private SuperTabControlPanel superTabControlPanel2;
		private SuperTabItem superTabItemComplete;
		private OLVColumn olvColumnSpeed;
		public ObjectListView listViewDownloadingList;
		private OLVColumn olvColumnProgress;
		private BarRenderer albumProgressBar;
		private ExpandablePanel AlbumDetailPannel;
		private ObjectListView AlbumDownloadDetaiView;
		private OLVColumn olvColumnSongName;
		private OLVColumn olvColumnSongArtist;
		private OLVColumn olvColumnSongSize;
		private OLVColumn olvColumnSongSpeed;
		private OLVColumn olvColumnSongProgress;
		private SuperTabControlPanel superTabControlPanel3;
		private SuperTabItem superTabItemAll;
		private Bar bar1;
		private ControlContainerItem controlContainerItem1;
		private ControlContainerItem controlContainerItem2;
		private ControlContainerItem controlContainerItem3;
		private ControlContainerItem controlContainerItem4;
		private ControlContainerItem controlContainerItem5;
		private ButtonItem buttonNewTask;
		private ButtonItem buttonStart;
		private ButtonItem buttonPause;
		private ButtonItem buttonOpenFolder;
		private ButtonItem buttonDelete;
		private OLVColumn olvColumnAlbum;
		private OLVColumn olvColumnArtist;
		private OLVColumn olvColumnYear;
		private OLVColumn olvColumnStatus;
		protected internal ImageRenderer imageRendererCover;
		private LabelX AlbumYearLabel;
		private LabelX ArtistLabel;
		private LabelX AlbumNamelLabel;
		public ObjectListView listViewCompleted;
		private OLVColumn olvColumnFinishedAlbum;
		private OLVColumn olvColumnFinishedStatus;
		private OLVColumn olvColumn3;
		private OLVColumn olvColumn4;
		private OLVColumn olvColumn5;
		private OLVColumn olvColumn6;
		public ObjectListView listViewAll;
		private OLVColumn olvColumn7;
		private OLVColumn olvColumnAllStatus;
		private OLVColumn olvColumn9;
		private OLVColumn olvColumn10;
		private OLVColumn olvColumnAllSpeed;
		private OLVColumn olvColumn12;
		private DataListView dataListView1;
		private OLVColumn olvColumnSongStatus;
		private ButtonItem buttonItemConfig;
		private LinkLabel downloadURLLink;
		private CheckBoxItem clipboardMonitorCheckbox;
		private CheckBoxItem notifyPopoutChecbox;
		private Panel panel1;
		private Panel panel2;
		[DllImport("User32.dll")]
		protected static extern int SetClipboardViewer(int hWndNewViewer);
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
		protected override void Dispose(bool disposing)
		{
			LongkeyMusicForm.ChangeClipboardChain(base.Handle, this.nextClipboardViewer);
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		protected override void WndProc(ref Message m)
		{
			if (Preference.Instance.IsMonitorClipboard)
			{
				int msg = m.Msg;
				if (msg != 776)
				{
					if (msg != 781)
					{
						base.WndProc(ref m);
						return;
					}
					if (m.WParam == this.nextClipboardViewer)
					{
						this.nextClipboardViewer = m.LParam;
						return;
					}
					LongkeyMusicForm.SendMessage(this.nextClipboardViewer, m.Msg, m.WParam, m.LParam);
					return;
				}
				else
				{
					LongkeyMusicForm.SendMessage(this.nextClipboardViewer, m.Msg, m.WParam, m.LParam);
					IDataObject dataObject = Clipboard.GetDataObject();
					if (this._isFormInitialized && dataObject.GetDataPresent(DataFormats.Text))
					{
						string text = (string)dataObject.GetData(DataFormats.Text);
						if (Util.IsValidAlbumUrl(text) != Util.AlbumType.Unrecognize && TaskForm.Instance.textboxAlbumUrl.Text.IndexOf(text) == -1)
						{
							if (TaskForm.Instance.textboxAlbumUrl.Text != "")
							{
								TextBox expr_CA = TaskForm.Instance.textboxAlbumUrl;
								expr_CA.Text += Environment.NewLine;
							}
							TextBox expr_E9 = TaskForm.Instance.textboxAlbumUrl;
							expr_E9.Text += text;
							this.OpenDialog(TaskForm.Instance);
							return;
						}
					}
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}
		private void listViewDownloadingList_FormatCell(object sender, FormatCellEventArgs e)
		{
			AlbumMeta albumMeta = (AlbumMeta)e.Model;
			if (e.ColumnIndex == 0)
			{
				if (albumMeta.Album != null)
				{
					e.SubItem.Decoration = new AlbumDecoration(albumMeta);
					return;
				}
				string text = "正在获取'" + albumMeta.AlbumKey + "'信息";
				e.SubItem.Decoration = new TextDecoration(text, ContentAlignment.MiddleCenter);
			}
		}
		private void CheckUpdate()
		{
			UpdateManager.Instance.PingServer(new SetTwoMessageHandler(this.SetDownloadURLLink));
		}
		private void SetDownloadURLLink(string anchorText, string link)
		{
			if (this.downloadURLLink.InvokeRequired)
			{
				SetTwoMessageHandler method = delegate(string anchorMsg, string linkMsg)
				{
					this.downloadURLLink.Text = anchorMsg;
					this.downloadURLLink.Links.Clear();
					this.downloadURLLink.Links.Add(0, linkMsg.Length, linkMsg);
				};
				base.Invoke(method, new object[]
				{
					anchorText
				});
				return;
			}
			this.downloadURLLink.Text = anchorText;
			this.downloadURLLink.Links.Clear();
			this.downloadURLLink.Links.Add(0, link.Length, link);
		}
		private void downloadURLLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string text = e.Link.LinkData as string;
			if (text != null && text.StartsWith("http://"))
			{
				Process.Start(text);
			}
		}
		private void DownloadFinishNotification(AlbumMeta album)
		{
			if (Preference.Instance.IsNotify)
			{
				base.Invoke((MethodInvoker)delegate
				{
					DownloadFinishToolTipForm.Instance.SetForm(album);
					DownloadFinishToolTipForm.Instance.Show();
				});
			}
		}
		private void InitManagers()
		{
			Dictionary<DataAndUIMaster.ListType, ObjectListView> dictionary = new Dictionary<DataAndUIMaster.ListType, ObjectListView>();
			dictionary.Add(DataAndUIMaster.ListType.ALL, this.listViewAll);
			dictionary.Add(DataAndUIMaster.ListType.DOWNLOADING, this.listViewDownloadingList);
			dictionary.Add(DataAndUIMaster.ListType.FINISHED, this.listViewCompleted);
			Dictionary<DataAndUIMaster.ListType, SetIntHandler> dictionary2 = new Dictionary<DataAndUIMaster.ListType, SetIntHandler>();
			dictionary2.Add(DataAndUIMaster.ListType.ALL, new SetIntHandler(this.SetAllTabNum));
			dictionary2.Add(DataAndUIMaster.ListType.DOWNLOADING, new SetIntHandler(this.SetDownloadingTabNum));
			dictionary2.Add(DataAndUIMaster.ListType.FINISHED, new SetIntHandler(this.SetFinishTabNum));
			DataAndUIMaster.Instance.SetAlbumList(dictionary);
			DataAndUIMaster.Instance.SetSelectedSongList(this.AlbumDownloadDetaiView);
			DataAndUIMaster.Instance.SetRefreshUIDelegate(new RefreshAlbumDetail(this.RefreshUI));
			DataAndUIMaster.Instance.SetSetTabNumHandlers(dictionary2);
			DataAndUIMaster.Instance.SetDownloadFinishNotification(new SetAlbumHandler(this.DownloadFinishNotification));
			AlbumFactory.Instance.AddObserver(DataAndUIMaster.Instance);
			AlbumFactory.Instance.AddObserver(AlbumQueueDownloadManager.Instance);
			SongFactory.Instance.AddObserver(DataAndUIMaster.Instance);
			DataAndUIMaster.Instance.LoadHistory();
		}
		private void InitQueueView()
		{
			AspectToStringConverterDelegate aspectToStringConverter = delegate(object x)
			{
				long num = (long)x;
				int[] array = new int[]
				{
					1073741824,
					1048576,
					1024
				};
				string[] array2 = new string[]
				{
					"GB",
					"MB",
					"KB"
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (num >= (long)array[i])
					{
						return string.Format("{0:#,##0.##} " + array2[i], (double)num / (double)array[i]);
					}
				}
				return "";
			};
			AspectToStringConverterDelegate aspectToStringConverter2 = delegate(object x)
			{
				long num = (long)x;
				int[] array = new int[]
				{
					1073741824,
					1048576,
					1024
				};
				string[] array2 = new string[]
				{
					"GB/s",
					"MB/s",
					"KB/s"
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (num >= (long)array[i])
					{
						return string.Format("{0:#,##0.##} " + array2[i], (double)num / (double)array[i]);
					}
				}
				if (num == 0L)
				{
					return "";
				}
				return "0 KB/s";
			};
			this.olvColumnStatus.ImageGetter = new ImageGetterDelegate(this.StatusImageGetter);
			this.olvColumnAllStatus.ImageGetter = new ImageGetterDelegate(this.StatusImageGetter);
			this.olvColumnFinishedStatus.ImageGetter = new ImageGetterDelegate(this.StatusImageGetter);
			this.olvColumnSongStatus.ImageGetter = new ImageGetterDelegate(this.SongDownloaderStatusImageGetter);
			this.olvColumnSongSize.AspectToStringConverter = aspectToStringConverter;
			this.olvColumnSongSpeed.AspectToStringConverter = aspectToStringConverter2;
			this.olvColumnSpeed.AspectToStringConverter = aspectToStringConverter2;
			this.olvColumnAllSpeed.AspectToStringConverter = aspectToStringConverter2;
			this.listViewDownloadingList.SelectionChanged += new EventHandler(this.listSelectionChanged);
			this.listViewAll.SelectionChanged += new EventHandler(this.listSelectionChanged);
			this.listViewCompleted.SelectionChanged += new EventHandler(this.listSelectionChanged);
			this.superTabControl1.SelectedTabChanged += new EventHandler<SuperTabStripSelectedTabChangedEventArgs>(this.tabSelectionChanged);
		}
		private void RefreshUI(AlbumMeta album)
		{
			this.RefreshDownloadDetailTitle(album);
			this.RefreshMenualButtonStatus(album);
		}
		private void tabSelectionChanged(object sender, EventArgs e)
		{
			this.listViewAll.SelectedIndex = -1;
			this.listViewDownloadingList.SelectedIndex = -1;
			this.listViewCompleted.SelectedIndex = -1;
		}
		private void listSelectionChanged(object sender, EventArgs e)
		{
			ObjectListView objectListView = (ObjectListView)sender;
			AlbumMeta albumMeta = (AlbumMeta)objectListView.SelectedObject;
			this.RefreshDownloadDetailTitle(albumMeta);
			this.RefreshMenualButtonStatus(albumMeta);
			if (albumMeta != null)
			{
				DataAndUIMaster.Instance.ResetSongs(albumMeta.AlbumKey);
				DataAndUIMaster.Instance.CurrentSelectingAlbum = albumMeta;
				return;
			}
			DataAndUIMaster.Instance.CurrentSelectingAlbum = albumMeta;
		}
		private object SongDownloaderStatusImageGetter(object x)
		{
			SongMeta songMeta = (SongMeta)x;
			switch (songMeta.DownloadStatus)
			{
			case SongMeta.Status.DOWNLOADING:
				return Resources.downloading_16;
			case SongMeta.Status.COMPLETED:
				return Resources.done_16;
			case SongMeta.Status.FAILED:
				return Resources.error_16;
			case SongMeta.Status.RETRYING:
				return Resources.retry_16;
			case SongMeta.Status.STOPPED:
				return Resources.pause_16;
			default:
				return Resources.waiting_16;
			}
		}
		private object StatusImageGetter(object x)
		{
			AlbumMeta albumMeta = (AlbumMeta)x;
			switch (albumMeta.DownloadStatus)
			{
			case AlbumMeta.Status.Waiting:
				return Resources.waiting;
			case AlbumMeta.Status.Downloading:
				return Resources.Downloading;
			case AlbumMeta.Status.Stopping:
				return Resources.pausing;
			case AlbumMeta.Status.Stopped:
				return Resources.pause;
			case AlbumMeta.Status.Finished:
				return Resources.complete;
			default:
				return Resources.waiting;
			}
		}
		private void ResetAlbumSongs(IEnumerable<SongMeta> collection)
		{
			this.AlbumDownloadDetaiView.SetObjects(collection);
			this.AlbumDownloadDetaiView.BuildList();
		}
		private void RefreshObject(object obj)
		{
			if (this.listViewDownloadingList.InvokeRequired)
			{
				RefreshListRow method = new RefreshListRow(this.listViewDownloadingList.RefreshObject);
				base.Invoke(method, new object[]
				{
					obj
				});
				return;
			}
			this.listViewDownloadingList.RefreshObject(obj);
		}
		public void InitUIs()
		{
			this.clipboardMonitorCheckbox.Checked = Preference.Instance.IsMonitorClipboard;
			this.notifyPopoutChecbox.Checked = Preference.Instance.IsNotify;
		}
		public LongkeyMusicForm()
		{
			this.InitializeComponent();
			this.nextClipboardViewer = (IntPtr)LongkeyMusicForm.SetClipboardViewer((int)base.Handle);
			this.InitQueueView();
			this.InitManagers();
			this.InitUIs();
			this.CheckUpdate();
			this._isFormInitialized = true;
			this.Text = "LongkeyMusic " + Application.ProductVersion;
		}
		public void SaveToConfigFile(string key, string value)
		{
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			if (ConfigurationManager.AppSettings[key] != null)
			{
				configuration.AppSettings.Settings.Remove(key);
			}
			configuration.AppSettings.Settings.Add(key, value);
			configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}
		private void SetButton(ButtonItem btn, bool isEnable)
		{
			if (btn.InvokeRequired)
			{
				SetBoolHandler method = delegate(bool isTrue)
				{
					btn.Enabled = isTrue;
				};
				base.Invoke(method, new object[]
				{
					isEnable
				});
				return;
			}
			btn.Enabled = isEnable;
		}
		private void RefreshMenualButtonStatus(AlbumMeta album)
		{
			this.SetButton(this.buttonPause, false);
			this.SetButton(this.buttonStart, false);
			this.SetButton(this.buttonDelete, false);
			this.SetButton(this.buttonOpenFolder, false);
			if (album != null)
			{
				switch (album.DownloadStatus)
				{
				case AlbumMeta.Status.Waiting:
					this.SetButton(this.buttonDelete, true);
					this.SetButton(this.buttonPause, true);
					return;
				case AlbumMeta.Status.Downloading:
					this.SetButton(this.buttonPause, true);
					return;
				case AlbumMeta.Status.Stopping:
					this.SetButton(this.buttonPause, true);
					break;
				case AlbumMeta.Status.Stopped:
					this.SetButton(this.buttonDelete, true);
					this.SetButton(this.buttonStart, true);
					return;
				case AlbumMeta.Status.Finished:
					this.SetButton(this.buttonDelete, true);
					this.SetButton(this.buttonOpenFolder, true);
					return;
				default:
					return;
				}
			}
		}
		private void RefreshDownloadDetailTitle(AlbumMeta meta)
		{
			if (meta != null)
			{
				if (this.pictureboxCoverPicture.InvokeRequired)
				{
					SetPictureBoxCoverPicture method = delegate(Bitmap pic)
					{
						this.pictureboxCoverPicture.Image = pic;
					};
					if (meta.CoverThumnail != null)
					{
						base.Invoke(method, new object[]
						{
							meta.CoverThumnail
						});
					}
				}
				else
				{
					if (meta.CoverThumnail != null)
					{
						this.pictureboxCoverPicture.Image = meta.CoverThumnail;
					}
				}
				if (this.AlbumNamelLabel.InvokeRequired)
				{
					SetMessageHandler method2 = delegate(string album)
					{
						this.AlbumNamelLabel.Text = album;
					};
					base.Invoke(method2, new object[]
					{
						meta.Album
					});
				}
				else
				{
					this.AlbumNamelLabel.Text = meta.Album;
				}
				if (this.ArtistLabel.InvokeRequired)
				{
					SetMessageHandler method3 = delegate(string artist)
					{
						this.ArtistLabel.Text = artist;
					};
					base.Invoke(method3, new object[]
					{
						meta.Artist
					});
				}
				else
				{
					this.ArtistLabel.Text = meta.Artist;
				}
				if (this.AlbumYearLabel.InvokeRequired)
				{
					SetMessageHandler method4 = delegate(string year)
					{
						this.AlbumYearLabel.Text = year;
					};
					base.Invoke(method4, new object[]
					{
						meta.Year.ToString()
					});
				}
				else
				{
					this.AlbumYearLabel.Text = meta.Year.ToString();
				}
				this.SetDetailPannelVisible(true);
				this.SetFormWidth();
				return;
			}
			this.SetDetailPannelVisible(false);
			this.SetFormWidth();
		}
		private void SetFormWidth()
		{
			int num = this.AlbumDetailPannel.Visible ? (this.panel1.Width + this.panel2.Width) : this.panel1.Width;
			if (base.InvokeRequired)
			{
				SetIntHandler method = delegate(int width)
				{
					base.Width = width;
				};
				base.Invoke(method, new object[]
				{
					num
				});
				return;
			}
			base.Width = num;
		}
		private void SetAllTabNum(int num)
		{
			string text = "所有任务 (" + num.ToString() + ")";
			if (this.superTabItemAll.InvokeRequired)
			{
				SetMessageHandler method = delegate(string message)
				{
					this.superTabItemAll.Text = message;
				};
				base.Invoke(method, new object[]
				{
					text
				});
				return;
			}
			this.superTabItemAll.Text = text;
		}
		private void SetDownloadingTabNum(int num)
		{
			string text = "正在下载 (" + num.ToString() + ")";
			SuperTabItem st = this.superTabItemDownloadingTab;
			if (st.InvokeRequired)
			{
				SetMessageHandler method = delegate(string message)
				{
					st.Text = message;
				};
				base.Invoke(method, new object[]
				{
					text
				});
				return;
			}
			st.Text = text;
		}
		private void SetFinishTabNum(int num)
		{
			string text = "已经完成 (" + num.ToString() + ")";
			SuperTabItem st = this.superTabItemComplete;
			if (st.InvokeRequired)
			{
				SetMessageHandler method = delegate(string message)
				{
					st.Text = message;
				};
				base.Invoke(method, new object[]
				{
					text
				});
				return;
			}
			st.Text = text;
		}
		private void SetDetailPannelVisible(bool isVisible)
		{
			if (this.AlbumDetailPannel.InvokeRequired)
			{
				SetBoolHandler method = delegate(bool isTrue)
				{
					this.panel2.Visible = isTrue;
				};
				base.Invoke(method, new object[]
				{
					isVisible
				});
				return;
			}
			this.panel2.Visible = isVisible;
		}
		private void formSizeChange(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				base.Hide();
				this.notifyIcon.Visible = true;
			}
		}
		private void notifyIcon_Click(object sender, EventArgs e)
		{
			base.TopMost = true;
			base.Visible = true;
			base.WindowState = FormWindowState.Normal;
			base.Focus();
		}
		private void LongkeyMusicForm_LostFocus(object sender, EventArgs e)
		{
			base.TopMost = false;
		}
		private DialogResult OpenDialog(Form form)
		{
			base.Invoke((MethodInvoker)delegate
			{
				form.TopMost = true;
				if (!form.Visible)
				{
					form.ShowDialog();
				}
			});
			return form.DialogResult;
		}
		private void buttonNewTask_Click(object sender, EventArgs e)
		{
			this.OpenDialog(TaskForm.Instance);
		}
		private void buttonPause_Click(object sender, EventArgs e)
		{
			AlbumMeta albumMeta = null;
			switch (this.superTabControl1.SelectedTabIndex)
			{
			case 0:
				albumMeta = (AlbumMeta)this.listViewAll.SelectedObject;
				break;
			case 1:
				albumMeta = (AlbumMeta)this.listViewDownloadingList.SelectedObject;
				break;
			case 2:
				albumMeta = (AlbumMeta)this.listViewCompleted.SelectedObject;
				break;
			}
			if (albumMeta != null)
			{
				this.buttonStart.Enabled = true;
				this.buttonPause.Enabled = false;
				AlbumQueueDownloadManager.Instance.StopTask(albumMeta);
			}
		}
		private void buttonStart_Click(object sender, EventArgs e)
		{
			AlbumMeta albumMeta = null;
			switch (this.superTabControl1.SelectedTabIndex)
			{
			case 0:
				albumMeta = (AlbumMeta)this.listViewAll.SelectedObject;
				break;
			case 1:
				albumMeta = (AlbumMeta)this.listViewDownloadingList.SelectedObject;
				break;
			case 2:
				albumMeta = (AlbumMeta)this.listViewCompleted.SelectedObject;
				break;
			}
			if (albumMeta != null)
			{
				this.buttonStart.Enabled = false;
				this.buttonPause.Enabled = true;
				AlbumQueueDownloadManager.Instance.AddTask(albumMeta);
			}
		}
		private void buttonOpenFolder_Click(object sender, EventArgs e)
		{
			AlbumMeta albumMeta = null;
			switch (this.superTabControl1.SelectedTabIndex)
			{
			case 0:
				albumMeta = (AlbumMeta)this.listViewAll.SelectedObject;
				break;
			case 1:
				albumMeta = (AlbumMeta)this.listViewDownloadingList.SelectedObject;
				break;
			case 2:
				albumMeta = (AlbumMeta)this.listViewCompleted.SelectedObject;
				break;
			}
			if (albumMeta != null)
			{
				try
				{
					Process.Start(Util.GetAlbumDir(albumMeta));
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}
		private void buttonDelete_Click(object sender, EventArgs e)
		{
			ArrayList arrayList = new ArrayList();
			switch (this.superTabControl1.SelectedTabIndex)
			{
			case 0:
				arrayList = (ArrayList)this.listViewAll.SelectedObjects;
				break;
			case 1:
				arrayList = (ArrayList)this.listViewDownloadingList.SelectedObjects;
				break;
			case 2:
				arrayList = (ArrayList)this.listViewCompleted.SelectedObjects;
				break;
			}
			if (arrayList.Count > 0)
			{
				DialogResult dialogResult = DeleteConfirmBox.Instance.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					if (dialogResult != DialogResult.Yes)
					{
						return;
					}
				}
				else
				{
					IEnumerator enumerator = arrayList.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object current = enumerator.Current;
							AlbumMeta album = (AlbumMeta)current;
							AlbumQueueDownloadManager.Instance.DeleteTask(album, false);
						}
						return;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				foreach (object current2 in arrayList)
				{
					AlbumMeta album2 = (AlbumMeta)current2;
					AlbumQueueDownloadManager.Instance.DeleteTask(album2, true);
				}
			}
		}
		private void clipboardMonitorCheckbox_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
		{
			Preference.Instance.IsMonitorClipboard = this.clipboardMonitorCheckbox.Checked;
			Preference.Instance.Serialize();
		}
		private void notifyPopoutChecbox_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
		{
			Preference.Instance.IsNotify = this.notifyPopoutChecbox.Checked;
			Preference.Instance.Serialize();
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LongkeyMusicForm));
			this.folderbrowserdialogBasePath = new FolderBrowserDialog();
			this.notifyIcon = new NotifyIcon(this.components);
			this.superTabControl1 = new SuperTabControl();
			this.superTabControlPanel3 = new SuperTabControlPanel();
			this.listViewAll = new ObjectListView();
			this.olvColumn7 = new OLVColumn();
			this.olvColumnAllStatus = new OLVColumn();
			this.imageRendererCover = new ImageRenderer();
			this.olvColumn9 = new OLVColumn();
			this.olvColumn10 = new OLVColumn();
			this.olvColumnAllSpeed = new OLVColumn();
			this.olvColumn12 = new OLVColumn();
			this.superTabItemAll = new SuperTabItem();
			this.superTabControlPanel2 = new SuperTabControlPanel();
			this.listViewCompleted = new ObjectListView();
			this.olvColumnFinishedAlbum = new OLVColumn();
			this.olvColumnFinishedStatus = new OLVColumn();
			this.olvColumn3 = new OLVColumn();
			this.olvColumn4 = new OLVColumn();
			this.olvColumn5 = new OLVColumn();
			this.olvColumn6 = new OLVColumn();
			this.superTabItemComplete = new SuperTabItem();
			this.superTabControlPanel1 = new SuperTabControlPanel();
			this.listViewDownloadingList = new ObjectListView();
			this.olvColumnAlbum = new OLVColumn();
			this.olvColumnStatus = new OLVColumn();
			this.olvColumnArtist = new OLVColumn();
			this.olvColumnYear = new OLVColumn();
			this.olvColumnSpeed = new OLVColumn();
			this.olvColumnProgress = new OLVColumn();
			this.dataListView1 = new DataListView();
			this.superTabItemDownloadingTab = new SuperTabItem();
			this.albumProgressBar = new BarRenderer();
			this.superTabItem1 = new SuperTabItem();
			this.AlbumDetailPannel = new ExpandablePanel();
			this.AlbumYearLabel = new LabelX();
			this.ArtistLabel = new LabelX();
			this.AlbumNamelLabel = new LabelX();
			this.AlbumDownloadDetaiView = new ObjectListView();
			this.olvColumnSongName = new OLVColumn();
			this.olvColumnSongArtist = new OLVColumn();
			this.olvColumnSongSize = new OLVColumn();
			this.olvColumnSongSpeed = new OLVColumn();
			this.olvColumnSongProgress = new OLVColumn();
			this.olvColumnSongStatus = new OLVColumn();
			this.pictureboxCoverPicture = new PictureBox();
			this.bar1 = new Bar();
			this.downloadURLLink = new LinkLabel();
			this.buttonNewTask = new ButtonItem();
			this.buttonStart = new ButtonItem();
			this.buttonPause = new ButtonItem();
			this.buttonOpenFolder = new ButtonItem();
			this.buttonDelete = new ButtonItem();
			this.buttonItemConfig = new ButtonItem();
			this.clipboardMonitorCheckbox = new CheckBoxItem();
			this.notifyPopoutChecbox = new CheckBoxItem();
			this.controlContainerItem1 = new ControlContainerItem();
			this.controlContainerItem2 = new ControlContainerItem();
			this.controlContainerItem3 = new ControlContainerItem();
			this.controlContainerItem4 = new ControlContainerItem();
			this.controlContainerItem5 = new ControlContainerItem();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
			((ISupportInitialize)this.superTabControl1).BeginInit();
			this.superTabControl1.SuspendLayout();
			this.superTabControlPanel3.SuspendLayout();
			((ISupportInitialize)this.listViewAll).BeginInit();
			this.superTabControlPanel2.SuspendLayout();
			((ISupportInitialize)this.listViewCompleted).BeginInit();
			this.superTabControlPanel1.SuspendLayout();
			((ISupportInitialize)this.listViewDownloadingList).BeginInit();
			((ISupportInitialize)this.dataListView1).BeginInit();
			this.AlbumDetailPannel.SuspendLayout();
			((ISupportInitialize)this.AlbumDownloadDetaiView).BeginInit();
			((ISupportInitialize)this.pictureboxCoverPicture).BeginInit();
			((ISupportInitialize)this.bar1).BeginInit();
			this.bar1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.notifyIcon.Icon = (Icon)componentResourceManager.GetObject("notifyIcon.Icon");
			this.notifyIcon.Text = "Longkey 音乐下载";
			this.notifyIcon.Visible = true;
			this.notifyIcon.Click += new EventHandler(this.notifyIcon_Click);
			this.superTabControl1.ControlBox.CloseBox.Name = "";
			this.superTabControl1.ControlBox.MenuBox.Name = "";
			this.superTabControl1.ControlBox.Name = "";
			this.superTabControl1.ControlBox.SubItems.AddRange(new BaseItem[]
			{
				this.superTabControl1.ControlBox.MenuBox,
				this.superTabControl1.ControlBox.CloseBox
			});
			this.superTabControl1.Controls.Add(this.superTabControlPanel3);
			this.superTabControl1.Controls.Add(this.superTabControlPanel2);
			this.superTabControl1.Controls.Add(this.superTabControlPanel1);
			this.superTabControl1.Location = new Point(3, 39);
			this.superTabControl1.Name = "superTabControl1";
			this.superTabControl1.ReorderTabsEnabled = true;
			this.superTabControl1.SelectedTabFont = new Font("SimSun", 9f, FontStyle.Bold);
			this.superTabControl1.SelectedTabIndex = 1;
			this.superTabControl1.Size = new Size(644, 484);
			this.superTabControl1.TabAlignment = eTabStripAlignment.Left;
			this.superTabControl1.TabFont = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.superTabControl1.TabIndex = 0;
			this.superTabControl1.Tabs.AddRange(new BaseItem[]
			{
				this.superTabItemAll,
				this.superTabItemDownloadingTab,
				this.superTabItemComplete
			});
			this.superTabControl1.TabStyle = eSuperTabStyle.Office2010BackstageBlue;
			this.superTabControl1.Text = "superTabControl1";
			this.superTabControlPanel3.Controls.Add(this.listViewAll);
			this.superTabControlPanel3.Dock = DockStyle.Fill;
			this.superTabControlPanel3.Location = new Point(76, 0);
			this.superTabControlPanel3.Name = "superTabControlPanel3";
			this.superTabControlPanel3.Size = new Size(568, 484);
			this.superTabControlPanel3.TabIndex = 0;
			this.superTabControlPanel3.TabItem = this.superTabItemAll;
			this.listViewAll.AllColumns.Add(this.olvColumn7);
			this.listViewAll.AllColumns.Add(this.olvColumnAllStatus);
			this.listViewAll.AllColumns.Add(this.olvColumn9);
			this.listViewAll.AllColumns.Add(this.olvColumn10);
			this.listViewAll.AllColumns.Add(this.olvColumnAllSpeed);
			this.listViewAll.AllColumns.Add(this.olvColumn12);
			this.listViewAll.BorderStyle = BorderStyle.None;
			this.listViewAll.Columns.AddRange(new ColumnHeader[]
			{
				this.olvColumn7,
				this.olvColumnAllStatus,
				this.olvColumn9,
				this.olvColumn10,
				this.olvColumnAllSpeed,
				this.olvColumn12
			});
			this.listViewAll.Dock = DockStyle.Fill;
			this.listViewAll.FullRowSelect = true;
			this.listViewAll.HighlightBackgroundColor = Color.Moccasin;
			this.listViewAll.Location = new Point(0, 0);
			this.listViewAll.Name = "listViewAll";
			this.listViewAll.OwnerDraw = true;
			this.listViewAll.RowHeight = 40;
			this.listViewAll.Size = new Size(568, 484);
			this.listViewAll.TabIndex = 1;
			this.listViewAll.UseAlternatingBackColors = true;
			this.listViewAll.UseCompatibleStateImageBehavior = false;
			this.listViewAll.View = View.Details;
			this.olvColumn7.AspectName = "Album";
			this.olvColumn7.DisplayIndex = 1;
			this.olvColumn7.ImageAspectName = "";
			this.olvColumn7.IsEditable = false;
			this.olvColumn7.Sortable = false;
			this.olvColumn7.Text = "专辑";
			this.olvColumn7.Width = 200;
			this.olvColumnAllStatus.AspectName = "Stataus";
			this.olvColumnAllStatus.DisplayIndex = 0;
			this.olvColumnAllStatus.Groupable = false;
			this.olvColumnAllStatus.Renderer = this.imageRendererCover;
			this.olvColumnAllStatus.Text = "";
			this.olvColumnAllStatus.Width = 32;
			this.olvColumn9.AspectName = "Artist";
			this.olvColumn9.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumn9.Text = "艺人";
			this.olvColumn9.Width = 100;
			this.olvColumn10.AspectName = "Year";
			this.olvColumn10.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumn10.Text = "年份";
			this.olvColumn10.TextAlign = HorizontalAlignment.Center;
			this.olvColumn10.Width = 50;
			this.olvColumnAllSpeed.AspectName = "Speed";
			this.olvColumnAllSpeed.AspectToStringFormat = "";
			this.olvColumnAllSpeed.Groupable = false;
			this.olvColumnAllSpeed.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnAllSpeed.IsEditable = false;
			this.olvColumnAllSpeed.Sortable = false;
			this.olvColumnAllSpeed.Text = "速度";
			this.olvColumnAllSpeed.TextAlign = HorizontalAlignment.Center;
			this.olvColumnAllSpeed.Width = 75;
			this.olvColumn12.AspectName = "Progress";
			this.olvColumn12.Groupable = false;
			this.olvColumn12.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumn12.Sortable = false;
			this.olvColumn12.Text = "进度";
			this.olvColumn12.TextAlign = HorizontalAlignment.Center;
			this.olvColumn12.Width = 90;
			this.superTabItemAll.AttachedControl = this.superTabControlPanel3;
			this.superTabItemAll.GlobalItem = false;
			this.superTabItemAll.Name = "superTabItemAll";
			this.superTabItemAll.Text = "全部任务";
			this.superTabControlPanel2.Controls.Add(this.listViewCompleted);
			this.superTabControlPanel2.Dock = DockStyle.Fill;
			this.superTabControlPanel2.Location = new Point(76, 0);
			this.superTabControlPanel2.Name = "superTabControlPanel2";
			this.superTabControlPanel2.Size = new Size(568, 484);
			this.superTabControlPanel2.TabIndex = 0;
			this.superTabControlPanel2.TabItem = this.superTabItemComplete;
			this.listViewCompleted.AllColumns.Add(this.olvColumnFinishedAlbum);
			this.listViewCompleted.AllColumns.Add(this.olvColumnFinishedStatus);
			this.listViewCompleted.AllColumns.Add(this.olvColumn3);
			this.listViewCompleted.AllColumns.Add(this.olvColumn4);
			this.listViewCompleted.AllColumns.Add(this.olvColumn5);
			this.listViewCompleted.AllColumns.Add(this.olvColumn6);
			this.listViewCompleted.BorderStyle = BorderStyle.FixedSingle;
			this.listViewCompleted.Columns.AddRange(new ColumnHeader[]
			{
				this.olvColumnFinishedAlbum,
				this.olvColumnFinishedStatus,
				this.olvColumn3,
				this.olvColumn4,
				this.olvColumn5,
				this.olvColumn6
			});
			this.listViewCompleted.Dock = DockStyle.Fill;
			this.listViewCompleted.FullRowSelect = true;
			this.listViewCompleted.HighlightBackgroundColor = Color.Moccasin;
			this.listViewCompleted.Location = new Point(0, 0);
			this.listViewCompleted.Name = "listViewCompleted";
			this.listViewCompleted.OwnerDraw = true;
			this.listViewCompleted.RowHeight = 40;
			this.listViewCompleted.Size = new Size(568, 484);
			this.listViewCompleted.TabIndex = 1;
			this.listViewCompleted.UseAlternatingBackColors = true;
			this.listViewCompleted.UseCompatibleStateImageBehavior = false;
			this.listViewCompleted.View = View.Details;
			this.olvColumnFinishedAlbum.AspectName = "Album";
			this.olvColumnFinishedAlbum.DisplayIndex = 1;
			this.olvColumnFinishedAlbum.ImageAspectName = "";
			this.olvColumnFinishedAlbum.IsEditable = false;
			this.olvColumnFinishedAlbum.Sortable = false;
			this.olvColumnFinishedAlbum.Text = "专辑";
			this.olvColumnFinishedAlbum.Width = 200;
			this.olvColumnFinishedStatus.AspectName = "Stataus";
			this.olvColumnFinishedStatus.DisplayIndex = 0;
			this.olvColumnFinishedStatus.Groupable = false;
			this.olvColumnFinishedStatus.Renderer = this.imageRendererCover;
			this.olvColumnFinishedStatus.Text = "";
			this.olvColumnFinishedStatus.Width = 32;
			this.olvColumn3.AspectName = "Artist";
			this.olvColumn3.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumn3.Text = "艺人";
			this.olvColumn3.Width = 100;
			this.olvColumn4.AspectName = "Year";
			this.olvColumn4.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumn4.Text = "年份";
			this.olvColumn4.TextAlign = HorizontalAlignment.Center;
			this.olvColumn4.Width = 50;
			this.olvColumn5.AspectName = "Tracks";
			this.olvColumn5.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumn5.Text = "曲目";
			this.olvColumn5.TextAlign = HorizontalAlignment.Center;
			this.olvColumn5.Width = 40;
			this.olvColumn6.AspectName = "FinishTime";
			this.olvColumn6.AspectToStringFormat = "{0:yyyy-MM-dd HH:mm}";
			this.olvColumn6.Groupable = false;
			this.olvColumn6.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumn6.Sortable = false;
			this.olvColumn6.Text = "完成时间";
			this.olvColumn6.TextAlign = HorizontalAlignment.Center;
			this.olvColumn6.Width = 120;
			this.superTabItemComplete.AttachedControl = this.superTabControlPanel2;
			this.superTabItemComplete.GlobalItem = false;
			this.superTabItemComplete.Name = "superTabItemComplete";
			this.superTabItemComplete.Text = "已完成";
			this.superTabControlPanel1.Controls.Add(this.listViewDownloadingList);
			this.superTabControlPanel1.Controls.Add(this.dataListView1);
			this.superTabControlPanel1.Dock = DockStyle.Fill;
			this.superTabControlPanel1.Location = new Point(76, 0);
			this.superTabControlPanel1.Name = "superTabControlPanel1";
			this.superTabControlPanel1.Size = new Size(568, 484);
			this.superTabControlPanel1.TabIndex = 1;
			this.superTabControlPanel1.TabItem = this.superTabItemDownloadingTab;
			this.listViewDownloadingList.AllColumns.Add(this.olvColumnAlbum);
			this.listViewDownloadingList.AllColumns.Add(this.olvColumnStatus);
			this.listViewDownloadingList.AllColumns.Add(this.olvColumnArtist);
			this.listViewDownloadingList.AllColumns.Add(this.olvColumnYear);
			this.listViewDownloadingList.AllColumns.Add(this.olvColumnSpeed);
			this.listViewDownloadingList.AllColumns.Add(this.olvColumnProgress);
			this.listViewDownloadingList.BorderStyle = BorderStyle.FixedSingle;
			this.listViewDownloadingList.Columns.AddRange(new ColumnHeader[]
			{
				this.olvColumnAlbum,
				this.olvColumnStatus,
				this.olvColumnArtist,
				this.olvColumnYear,
				this.olvColumnSpeed,
				this.olvColumnProgress
			});
			this.listViewDownloadingList.Dock = DockStyle.Fill;
			this.listViewDownloadingList.FullRowSelect = true;
			this.listViewDownloadingList.HighlightBackgroundColor = Color.Moccasin;
			this.listViewDownloadingList.Location = new Point(0, 0);
			this.listViewDownloadingList.Name = "listViewDownloadingList";
			this.listViewDownloadingList.OwnerDraw = true;
			this.listViewDownloadingList.RowHeight = 40;
			this.listViewDownloadingList.Size = new Size(568, 484);
			this.listViewDownloadingList.TabIndex = 0;
			this.listViewDownloadingList.UseAlternatingBackColors = true;
			this.listViewDownloadingList.UseCompatibleStateImageBehavior = false;
			this.listViewDownloadingList.View = View.Details;
			this.listViewDownloadingList.FormatCell += new EventHandler<FormatCellEventArgs>(this.listViewDownloadingList_FormatCell);
			this.olvColumnAlbum.AspectName = "Album";
			this.olvColumnAlbum.DisplayIndex = 1;
			this.olvColumnAlbum.ImageAspectName = "";
			this.olvColumnAlbum.IsEditable = false;
			this.olvColumnAlbum.Sortable = false;
			this.olvColumnAlbum.Text = "专辑";
			this.olvColumnAlbum.Width = 200;
			this.olvColumnStatus.AspectName = "Stataus";
			this.olvColumnStatus.DisplayIndex = 0;
			this.olvColumnStatus.Groupable = false;
			this.olvColumnStatus.Renderer = this.imageRendererCover;
			this.olvColumnStatus.Text = "";
			this.olvColumnStatus.Width = 32;
			this.olvColumnArtist.AspectName = "Artist";
			this.olvColumnArtist.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnArtist.Text = "艺人";
			this.olvColumnArtist.Width = 100;
			this.olvColumnYear.AspectName = "Year";
			this.olvColumnYear.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnYear.Text = "年份";
			this.olvColumnYear.TextAlign = HorizontalAlignment.Center;
			this.olvColumnYear.Width = 50;
			this.olvColumnSpeed.AspectName = "Speed";
			this.olvColumnSpeed.AspectToStringFormat = "";
			this.olvColumnSpeed.Groupable = false;
			this.olvColumnSpeed.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnSpeed.IsEditable = false;
			this.olvColumnSpeed.Sortable = false;
			this.olvColumnSpeed.Text = "速度";
			this.olvColumnSpeed.TextAlign = HorizontalAlignment.Center;
			this.olvColumnSpeed.Width = 75;
			this.olvColumnProgress.AspectName = "Progress";
			this.olvColumnProgress.Groupable = false;
			this.olvColumnProgress.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnProgress.Sortable = false;
			this.olvColumnProgress.Text = "进度";
			this.olvColumnProgress.TextAlign = HorizontalAlignment.Center;
			this.olvColumnProgress.Width = 90;
			this.dataListView1.DataSource = null;
			this.dataListView1.Location = new Point(-14, 451);
			this.dataListView1.Name = "dataListView1";
			this.dataListView1.Size = new Size(412, 227);
			this.dataListView1.TabIndex = 17;
			this.dataListView1.UseCompatibleStateImageBehavior = false;
			this.dataListView1.View = View.Details;
			this.superTabItemDownloadingTab.AttachedControl = this.superTabControlPanel1;
			this.superTabItemDownloadingTab.GlobalItem = false;
			this.superTabItemDownloadingTab.Name = "superTabItemDownloadingTab";
			this.superTabItemDownloadingTab.Text = "正在下载";
			this.albumProgressBar.FillColor = Color.Lime;
			this.albumProgressBar.FrameWidth = 300f;
			this.albumProgressBar.GradientEndColor = Color.PaleGreen;
			this.albumProgressBar.GradientStartColor = Color.Lime;
			this.superTabItem1.GlobalItem = false;
			this.superTabItem1.Name = "superTabItem1";
			this.superTabItem1.Text = "superTabItem1";
			this.AlbumDetailPannel.CanvasColor = SystemColors.Control;
			this.AlbumDetailPannel.CollapseDirection = eCollapseDirection.RightToLeft;
			this.AlbumDetailPannel.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			this.AlbumDetailPannel.Controls.Add(this.AlbumYearLabel);
			this.AlbumDetailPannel.Controls.Add(this.ArtistLabel);
			this.AlbumDetailPannel.Controls.Add(this.AlbumNamelLabel);
			this.AlbumDetailPannel.Controls.Add(this.AlbumDownloadDetaiView);
			this.AlbumDetailPannel.Controls.Add(this.pictureboxCoverPicture);
			this.AlbumDetailPannel.Dock = DockStyle.Fill;
			this.AlbumDetailPannel.ExpandButtonVisible = false;
			this.AlbumDetailPannel.Location = new Point(0, 0);
			this.AlbumDetailPannel.Margin = new System.Windows.Forms.Padding(0);
			this.AlbumDetailPannel.Name = "AlbumDetailPannel";
			this.AlbumDetailPannel.Size = new Size(595, 517);
			this.AlbumDetailPannel.Style.Alignment = StringAlignment.Center;
			this.AlbumDetailPannel.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
			this.AlbumDetailPannel.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
			this.AlbumDetailPannel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
			this.AlbumDetailPannel.Style.GradientAngle = 90;
			this.AlbumDetailPannel.TabIndex = 19;
			this.AlbumDetailPannel.TitleHeight = 40;
			this.AlbumDetailPannel.TitleStyle.Alignment = StringAlignment.Center;
			this.AlbumDetailPannel.TitleStyle.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
			this.AlbumDetailPannel.TitleStyle.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
			this.AlbumDetailPannel.TitleStyle.Border = eBorderType.RaisedInner;
			this.AlbumDetailPannel.TitleStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
			this.AlbumDetailPannel.TitleStyle.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
			this.AlbumDetailPannel.TitleStyle.GradientAngle = 90;
			this.AlbumDetailPannel.TitleText = "专辑详细信息";
			this.AlbumYearLabel.BackColor = Color.Transparent;
			this.AlbumYearLabel.BackgroundStyle.CornerType = eCornerType.Square;
			this.AlbumYearLabel.Location = new Point(130, 121);
			this.AlbumYearLabel.Name = "AlbumYearLabel";
			this.AlbumYearLabel.Size = new Size(441, 23);
			this.AlbumYearLabel.TabIndex = 13;
			this.ArtistLabel.BackColor = Color.Transparent;
			this.ArtistLabel.BackgroundStyle.CornerType = eCornerType.Square;
			this.ArtistLabel.Location = new Point(130, 83);
			this.ArtistLabel.Name = "ArtistLabel";
			this.ArtistLabel.Size = new Size(441, 23);
			this.ArtistLabel.TabIndex = 12;
			this.AlbumNamelLabel.BackColor = Color.Transparent;
			this.AlbumNamelLabel.BackgroundStyle.CornerType = eCornerType.Square;
			this.AlbumNamelLabel.Location = new Point(130, 44);
			this.AlbumNamelLabel.Name = "AlbumNamelLabel";
			this.AlbumNamelLabel.Size = new Size(441, 23);
			this.AlbumNamelLabel.Style = eDotNetBarStyle.Office2010;
			this.AlbumNamelLabel.TabIndex = 11;
			this.AlbumDownloadDetaiView.AllColumns.Add(this.olvColumnSongName);
			this.AlbumDownloadDetaiView.AllColumns.Add(this.olvColumnSongArtist);
			this.AlbumDownloadDetaiView.AllColumns.Add(this.olvColumnSongSize);
			this.AlbumDownloadDetaiView.AllColumns.Add(this.olvColumnSongSpeed);
			this.AlbumDownloadDetaiView.AllColumns.Add(this.olvColumnSongProgress);
			this.AlbumDownloadDetaiView.AllColumns.Add(this.olvColumnSongStatus);
			this.AlbumDownloadDetaiView.Columns.AddRange(new ColumnHeader[]
			{
				this.olvColumnSongName,
				this.olvColumnSongArtist,
				this.olvColumnSongSize,
				this.olvColumnSongSpeed,
				this.olvColumnSongProgress,
				this.olvColumnSongStatus
			});
			this.AlbumDownloadDetaiView.Location = new Point(0, 149);
			this.AlbumDownloadDetaiView.Name = "AlbumDownloadDetaiView";
			this.AlbumDownloadDetaiView.OwnerDraw = true;
			this.AlbumDownloadDetaiView.RowHeight = 20;
			this.AlbumDownloadDetaiView.Size = new Size(589, 368);
			this.AlbumDownloadDetaiView.TabIndex = 1;
			this.AlbumDownloadDetaiView.UseCompatibleStateImageBehavior = false;
			this.AlbumDownloadDetaiView.View = View.Details;
			this.olvColumnSongName.AspectName = "Song";
			this.olvColumnSongName.DisplayIndex = 1;
			this.olvColumnSongName.Groupable = false;
			this.olvColumnSongName.Text = "歌名";
			this.olvColumnSongName.Width = 200;
			this.olvColumnSongArtist.AspectName = "Artist";
			this.olvColumnSongArtist.DisplayIndex = 2;
			this.olvColumnSongArtist.Text = "艺人";
			this.olvColumnSongArtist.Width = 100;
			this.olvColumnSongSize.AspectName = "FileSize";
			this.olvColumnSongSize.DisplayIndex = 3;
			this.olvColumnSongSize.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnSongSize.Text = "文件大小";
			this.olvColumnSongSize.TextAlign = HorizontalAlignment.Center;
			this.olvColumnSongSize.Width = 72;
			this.olvColumnSongSpeed.AspectName = "Speed";
			this.olvColumnSongSpeed.DisplayIndex = 4;
			this.olvColumnSongSpeed.Groupable = false;
			this.olvColumnSongSpeed.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnSongSpeed.MaximumWidth = 75;
			this.olvColumnSongSpeed.MinimumWidth = 75;
			this.olvColumnSongSpeed.Searchable = false;
			this.olvColumnSongSpeed.Sortable = false;
			this.olvColumnSongSpeed.Text = "速度";
			this.olvColumnSongSpeed.TextAlign = HorizontalAlignment.Center;
			this.olvColumnSongSpeed.Width = 75;
			this.olvColumnSongProgress.AspectName = "Progress";
			this.olvColumnSongProgress.DisplayIndex = 5;
			this.olvColumnSongProgress.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnSongProgress.MaximumWidth = 90;
			this.olvColumnSongProgress.MinimumWidth = 90;
			this.olvColumnSongProgress.Renderer = this.albumProgressBar;
			this.olvColumnSongProgress.Text = "进度";
			this.olvColumnSongProgress.TextAlign = HorizontalAlignment.Center;
			this.olvColumnSongProgress.Width = 90;
			this.olvColumnSongStatus.AspectName = "DownloadStatus";
			this.olvColumnSongStatus.DisplayIndex = 0;
			this.olvColumnSongStatus.Groupable = false;
			this.olvColumnSongStatus.HeaderTextAlign = HorizontalAlignment.Center;
			this.olvColumnSongStatus.MaximumWidth = 20;
			this.olvColumnSongStatus.MinimumWidth = 20;
			this.olvColumnSongStatus.Renderer = this.imageRendererCover;
			this.olvColumnSongStatus.Text = "";
			this.olvColumnSongStatus.TextAlign = HorizontalAlignment.Center;
			this.olvColumnSongStatus.Width = 20;
			this.pictureboxCoverPicture.BackColor = Color.Transparent;
			this.pictureboxCoverPicture.InitialImage = null;
			this.pictureboxCoverPicture.Location = new Point(5, 44);
			this.pictureboxCoverPicture.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.pictureboxCoverPicture.Name = "pictureboxCoverPicture";
			this.pictureboxCoverPicture.Size = new Size(100, 100);
			this.pictureboxCoverPicture.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureboxCoverPicture.TabIndex = 10;
			this.pictureboxCoverPicture.TabStop = false;
			this.bar1.AccessibleDescription = "bar1 (bar1)";
			this.bar1.AccessibleName = "bar1";
			this.bar1.AccessibleRole = AccessibleRole.ToolBar;
			this.bar1.AntiAlias = true;
			this.bar1.Controls.Add(this.downloadURLLink);
			this.bar1.DockTabStripHeight = 32;
			this.bar1.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.bar1.Items.AddRange(new BaseItem[]
			{
				this.buttonNewTask,
				this.buttonStart,
				this.buttonPause,
				this.buttonOpenFolder,
				this.buttonDelete,
				this.buttonItemConfig
			});
			this.bar1.Location = new Point(77, 0);
			this.bar1.Name = "bar1";
			this.bar1.Size = new Size(570, 41);
			this.bar1.Stretch = true;
			this.bar1.Style = eDotNetBarStyle.StyleManagerControlled;
			this.bar1.TabIndex = 25;
			this.bar1.TabStop = false;
			this.bar1.Text = "bar1";
			this.downloadURLLink.AutoSize = true;
			this.downloadURLLink.BackColor = Color.Transparent;
			this.downloadURLLink.Location = new Point(296, 15);
			this.downloadURLLink.Name = "downloadURLLink";
			this.downloadURLLink.Size = new Size(0, 12);
			this.downloadURLLink.TabIndex = 0;
			this.downloadURLLink.VisitedLinkColor = Color.Blue;
			this.downloadURLLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.downloadURLLink_LinkClicked);
			this.buttonNewTask.Image = Resources.add;
			this.buttonNewTask.Name = "buttonNewTask";
			this.buttonNewTask.Tooltip = "新建下载任务";
			this.buttonNewTask.Click += new EventHandler(this.buttonNewTask_Click);
			this.buttonStart.Enabled = false;
			this.buttonStart.Image = Resources.start;
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Tooltip = "恢复下载";
			this.buttonStart.Click += new EventHandler(this.buttonStart_Click);
			this.buttonPause.Enabled = false;
			this.buttonPause.Image = Resources.pause;
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Text = "buttonItem1";
			this.buttonPause.Tooltip = "暂停下载";
			this.buttonPause.Click += new EventHandler(this.buttonPause_Click);
			this.buttonOpenFolder.Enabled = false;
			this.buttonOpenFolder.Image = Resources.open;
			this.buttonOpenFolder.Name = "buttonOpenFolder";
			this.buttonOpenFolder.Text = "buttonItem1";
			this.buttonOpenFolder.Tooltip = "打开专辑文件夹";
			this.buttonOpenFolder.Click += new EventHandler(this.buttonOpenFolder_Click);
			this.buttonDelete.Enabled = false;
			this.buttonDelete.Image = Resources.delete;
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Text = "buttonItem1";
			this.buttonDelete.Tooltip = "删除下载任务";
			this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
			this.buttonItemConfig.AutoCollapseOnClick = false;
			this.buttonItemConfig.AutoExpandOnClick = true;
			this.buttonItemConfig.ButtonStyle = eButtonStyle.ImageAndText;
			this.buttonItemConfig.Image = Resources.config;
			this.buttonItemConfig.Name = "buttonItemConfig";
			this.buttonItemConfig.SubItems.AddRange(new BaseItem[]
			{
				this.clipboardMonitorCheckbox,
				this.notifyPopoutChecbox
			});
			this.buttonItemConfig.Text = "设置";
			this.clipboardMonitorCheckbox.Name = "clipboardMonitorCheckbox";
			this.clipboardMonitorCheckbox.Text = "监视剪贴板";
			this.clipboardMonitorCheckbox.CheckedChanged += new CheckBoxChangeEventHandler(this.clipboardMonitorCheckbox_CheckedChanged);
			this.notifyPopoutChecbox.Name = "notifyPopoutChecbox";
			this.notifyPopoutChecbox.Text = "完成下载时弹出提示";
			this.notifyPopoutChecbox.CheckedChanged += new CheckBoxChangeEventHandler(this.notifyPopoutChecbox_CheckedChanged);
			this.controlContainerItem1.AllowItemResize = false;
			this.controlContainerItem1.MenuVisibility = eMenuVisibility.VisibleAlways;
			this.controlContainerItem1.Name = "controlContainerItem1";
			this.controlContainerItem2.AllowItemResize = false;
			this.controlContainerItem2.MenuVisibility = eMenuVisibility.VisibleAlways;
			this.controlContainerItem2.Name = "controlContainerItem2";
			this.controlContainerItem3.AllowItemResize = false;
			this.controlContainerItem3.MenuVisibility = eMenuVisibility.VisibleAlways;
			this.controlContainerItem3.Name = "controlContainerItem3";
			this.controlContainerItem4.AllowItemResize = false;
			this.controlContainerItem4.MenuVisibility = eMenuVisibility.VisibleAlways;
			this.controlContainerItem4.Name = "controlContainerItem4";
			this.controlContainerItem5.AllowItemResize = false;
			this.controlContainerItem5.MenuVisibility = eMenuVisibility.VisibleAlways;
			this.controlContainerItem5.Name = "controlContainerItem5";
			this.panel1.Controls.Add(this.bar1);
			this.panel1.Controls.Add(this.superTabControl1);
			this.panel1.Location = new Point(0, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(649, 517);
			this.panel1.TabIndex = 26;
			this.panel2.Controls.Add(this.AlbumDetailPannel);
			this.panel2.Location = new Point(653, 12);
			this.panel2.Name = "panel2";
			this.panel2.Size = new Size(595, 517);
			this.panel2.TabIndex = 27;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.GradientActiveCaption;
			base.ClientSize = new Size(649, 531);
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			base.MaximizeBox = false;
			base.Name = "LongkeyMusicForm";
			base.ShowInTaskbar = false;
			this.Text = "Longkey 音乐下载";
			base.SizeChanged += new EventHandler(this.formSizeChange);
			base.LostFocus += new EventHandler(this.LongkeyMusicForm_LostFocus);
			((ISupportInitialize)this.superTabControl1).EndInit();
			this.superTabControl1.ResumeLayout(false);
			this.superTabControlPanel3.ResumeLayout(false);
			((ISupportInitialize)this.listViewAll).EndInit();
			this.superTabControlPanel2.ResumeLayout(false);
			((ISupportInitialize)this.listViewCompleted).EndInit();
			this.superTabControlPanel1.ResumeLayout(false);
			((ISupportInitialize)this.listViewDownloadingList).EndInit();
			((ISupportInitialize)this.dataListView1).EndInit();
			this.AlbumDetailPannel.ResumeLayout(false);
			((ISupportInitialize)this.AlbumDownloadDetaiView).EndInit();
			((ISupportInitialize)this.pictureboxCoverPicture).EndInit();
			((ISupportInitialize)this.bar1).EndInit();
			this.bar1.ResumeLayout(false);
			this.bar1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
