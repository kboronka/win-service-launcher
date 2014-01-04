using System;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public class OnTimeOfDay : Schedule
	{
		private TimeSpan time;
		
		public OnTimeOfDay(Launcher parent) : base(parent)
		{
		}
		
		public OnTimeOfDay(Launcher parent, XML.Reader reader) : base(parent, reader)
		{
			time = reader.GetAttributeTimeSpan("time");
		}

		protected override void LaunchTick(Object state)
		{
			lock (this.launchTimer)
			{
				try
				{
					if (this.lastRun.TimeOfDay < this.time  && DateTime.Now.TimeOfDay >= this.time)
					{
						this.Launch();
					}
				}
				catch
				{

				}
				finally
				{
					long msToLaunch = Convert.ToInt32(this.time.TotalMilliseconds - DateTime.Now.TimeOfDay.TotalMilliseconds);
					
					if (msToLaunch < 0) msToLaunch = Convert.ToInt32(this.time.TotalMilliseconds);
					if (msToLaunch < 10) msToLaunch = 10;
					
					this.launchTimer.Change(msToLaunch, Timeout.Infinite);
				}
			}
		}
		
	}
}
