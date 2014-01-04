using System;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public abstract class Schedule
	{
		protected Launcher parent;
		protected DateTime lastRun;
		protected int loopRate = 100;
		
		public Schedule(Launcher parent)
		{
			this.parent = parent;
		}
		
		public Schedule(Launcher parent, XML.Reader reader)
		{
			this.parent = parent;
			this.lastRun = reader.GetAttributeTimestamp("lastrun");
		}
		
		#region Async
		
		protected System.Threading.Timer launchTimer;

		public void StartAsync()
		{
			lock (this.launchTimer)
			{
				this.launchTimer = new Timer(this.LaunchTick, null, 5000, Timeout.Infinite);
			}
		}
		
		public void StopAsync()
		{
			lock (this.launchTimer)
			{
				this.launchTimer.Dispose();
				this.launchTimer = null;
			}
		}

		protected abstract void LaunchTick(Object state);
		
		public void Launch()
		{
			try
			{
				WinServiceLauncher.Log("Launching " + this.parent.Filename + this.parent.Arguments);
				
				if (String.IsNullOrEmpty(this.parent.Domain))
				{
					ConsoleHelper.Start(this.parent.Filepath, this.parent.Arguments);
				}
				else
				{
					ConsoleHelper.StartAs(this.parent.Filepath, this.parent.Arguments, this.parent.Domain, this.parent.Username, this.parent.Password);
				}
				
				WinServiceLauncher.Log("Launching complete");
			}
			catch (Exception ex)
			{
				WinServiceLauncher.Log("Launching failed");
				WinServiceLauncher.Log(ex);
			}
		}
		
		#endregion
		
	}
}
