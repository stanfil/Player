using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Threading;
namespace Player
{
	static class Program
	{
		[DllImport("user32.dll", EntryPoint = "ShowWindow")]
		static extern bool ShowWindow(IntPtr handle, int flags);
		[DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
		static extern bool SetForegroundWindow(IntPtr handle);

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool isCreated;
			using(Mutex newMutex = new Mutex(true, @"Local\Player",out isCreated))
			{
				if (isCreated)
				{
					Application.EnableVisualStyles();
					using(RegistryKey subKey = Application.UserAppDataRegistry)
					{
						Form1 form = new Form1();
						subKey.SetValue("MyHandle", form.Handle);
						Application.Run(form);
					}
					newMutex.ReleaseMutex();
				}
				else
				{
					string text = string.Format("”{0}“应用程序已经运行。", AppDomain.CurrentDomain.FriendlyName);
					MessageBox.Show(text, "系统提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					using(RegistryKey subKey = Application.UserAppDataRegistry)
					{
						IntPtr handle = new IntPtr(Convert.ToInt32(subKey.GetValue("MyHandle")));
						ShowWindow(handle, 1);
						SetForegroundWindow(handle);
					}
				}
			}
		}
	}
}
