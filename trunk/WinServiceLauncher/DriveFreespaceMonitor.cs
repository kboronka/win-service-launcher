using System;
using System.IO;

using sar.Tools;
using sar.Socket;

namespace WinServiceLauncher
{
	public class DriveFreespaceMonitor : sar.Socket.SocketClient
	{
		private string machineName;
		
		public DriveFreespaceMonitor() : base("localhost", 8111, Program.ErrorLog, Program.DebugLog)
		{
			this.machineName = System.Environment.MachineName;
		}
		
		public DriveFreespaceMonitor(XML.Reader reader): base(reader.GetAttributeString("host"), int.Parse(reader.GetAttributeString("port")), Program.ErrorLog, Program.DebugLog)
		{
			this.machineName = reader.GetAttributeString("MachineName");
			
			if (String.IsNullOrEmpty(this.machineName))
			{
				this.machineName = System.Environment.MachineName;
			}
		}
		
		private void ReportFreeSpace()
		{
			foreach (DriveInfo drive in DriveInfo.GetDrives())
			{
				if (drive.IsReady && drive.DriveType == DriveType.Fixed)
				{
					string name = drive.Name;
					//drive.TotalFreeSpace;
				}
			}
		}
	}
}
