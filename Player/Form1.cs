using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLLGetFile;
using System.IO;
using Microsoft.Win32;
namespace Player
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			this.axWindowsMediaPlayer1.stretchToFit = true;
			this.axWindowsMediaPlayer1.settings.setMode("loop", true);
			using(RegistryKey userKey = Application.UserAppDataRegistry)
			{
				if((this.folderBrowserDialog1.SelectedPath = userKey.GetValue("MyMedia")as string) == null)
				{
					this.folderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
				}
				AddFiles();
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void AddFiles()
		{
			this.axWindowsMediaPlayer1.currentPlaylist.clear();
			listView1.Items.Clear();
			listView1.BeginUpdate();
			Class1 lf = new Class1();
			ListViewItem[] lvi = lf.searchDirectoryFile(this.folderBrowserDialog1.SelectedPath);
			for(int i=0; i < lvi.Length; i++)
			{
				if (lvi[i] == null)
				{
					break;
				}
				lvi[i].SubItems[0].Text = (listView1.Items.Count + 1).ToString();
				this.axWindowsMediaPlayer1.currentPlaylist.appendItem(this.axWindowsMediaPlayer1.newMedia(lvi[i].SubItems[2].Text));
				this.listView1.Items.Add(lvi[i]);
			}
			listView1.EndUpdate();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if(this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				AddFiles();
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.axWindowsMediaPlayer1.currentPlaylist.clear();
				listView1.Items.Clear();
				listView1.BeginUpdate();
				Class1 lf = new Class1();
				string[] strFile = this.openFileDialog1.FileNames;
				for(int i = 0; i < strFile.Length; i++)
				{
					ListViewItem lvi = lf.searchFile(strFile[i]);
					lvi.SubItems[0].Text = (listView1.Items.Count + 1).ToString();
					this.axWindowsMediaPlayer1.currentPlaylist.appendItem(this.axWindowsMediaPlayer1.newMedia(strFile[i]));
					this.listView1.Items.Add(lvi);
				}
				listView1.EndUpdate();
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			switch (this.axWindowsMediaPlayer1.playState)
			{
				case WMPLib.WMPPlayState.wmppsTransitioning:
				case WMPLib.WMPPlayState.wmppsPlaying:
					int index = -1;
					while (index < this.axWindowsMediaPlayer1.currentPlaylist.count)
					{
						if (this.axWindowsMediaPlayer1.currentMedia.get_isIdentical(this.axWindowsMediaPlayer1.currentPlaylist.get_Item(++index)))
						{
							this.listView1.Items[index].Selected = true;
							this.listView1.Focus();
							this.listView1.Items[index].EnsureVisible();
							this.Text = this.listView1.Items[index].SubItems[1].Text;
							break;
						}
					}
					break;
				case WMPLib.WMPPlayState.wmppsReady:
					this.axWindowsMediaPlayer1.Ctlcontrols.play();
					break;
			}
		}

		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.listView1.SelectedItems.Count > 0)
			{
				int iSel = this.listView1.SelectedItems[0].Index;
				this.Text = this.listView1.SelectedItems[0].SubItems[1].Text.Trim();
				if(iSel <= this.axWindowsMediaPlayer1.currentPlaylist.count)
				{
					this.axWindowsMediaPlayer1.Ctlcontrols.playItem(this.axWindowsMediaPlayer1.currentPlaylist.get_Item(iSel));
				}
			}
		}

		private void 播放列表折叠ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed;
			if (this.splitContainer1.Panel2Collapsed)
			{
				播放列表折叠ToolStripMenuItem.Text = "播放列表展开";
			}
			else{
				播放列表折叠ToolStripMenuItem.Text = "播放列表折叠";
			}
		}

		private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStripButton1_Click(null, null);
		}

		private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStripButton2_Click(null, null);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			using(RegistryKey subKey = Application.UserAppDataRegistry)
			{
				subKey.SetValue("MyMedia", this.folderBrowserDialog1.SelectedPath);
			}
		}

		private void 删除重复文件ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 删除错误文件ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 删除选择文件ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 删除全部文件ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 顺序播放ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            axWindowsMediaPlayer1.settings.setMode("showFrame", true);
		}

		private void 单曲循环ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            axWindowsMediaPlayer1.settings.setMode("loop", true);
		}

		private void 全部循环ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            axWindowsMediaPlayer1.settings.setMode("autoRewind", true);
        }

        private void 随机播放ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            axWindowsMediaPlayer1.settings.setMode("shuffle", true);
		}

        private void 删除文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            this.axWindowsMediaPlayer1.currentPlaylist.clear();
        }
    }
}
