/*
 * Created by SharpDevelop.
 * User: Boronka
 * Date: 1/3/2014
 * Time: 12:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public class OnInterval : Schedule
	{
		private long interval;		//ms
		
		public OnInterval(long interval)
		{
			this.interval = interval;
		}
		
		public OnInterval(XML.Reader reader)
		{
			this.interval = reader.GetAttributeLong("interval");
		}
		
		protected void LaunchTick(Object state)
		{
			lock (this.launchTimer)
			{
				try
				{
					if (DateTime.Now > this.lastRun.AddMilliseconds(this.interval))
					{
						this.Launch();
					}
				}
				catch
				{

				}
				finally
				{
					if (this.interval > 0)
					{
						this.launchTimer.Change(this.interval, Timeout.Infinite);
					}
				}
			}
		}
	}
}
