
using System;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public class OnInterval : Schedule
	{
		private long interval;		//ms
		
		public OnInterval(long interval) : base()
		{
			this.interval = interval;
		}
		
		public OnInterval(XML.Reader reader) : base(reader)
		{
			this.interval = reader.GetAttributeLong("interval");
		}
		
		protected override void LaunchTick(Object state)
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
