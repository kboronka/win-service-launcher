/*
 * Created by SharpDevelop.
 * User: Boronka
 * Date: 1/3/2014
 * Time: 12:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WinServiceLauncher.Launchers
{
	public abstract class Schedule
	{
		protected DateTime lastRun;
		protected int loopRate = 100;
		
		public Schedule()
		{
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
				WinServiceLauncher.Log("Launching " + this.filepath + this.arguments);
				
				if (String.IsNullOrEmpty(domain))
				{
					ConsoleHelper.Start(this.filepath, this.arguments);
				}
				else
				{
					ConsoleHelper.StartAs(this.filepath, this.arguments, this.domain, this.username, this.password);
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
