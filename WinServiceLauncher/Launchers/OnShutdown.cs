using System;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public class OnShutdown : Schedule
	{
		public OnShutdown(Launcher parent) : base(parent)
		{
		}
		
		public OnShutdown(XML.Reader reader) : base(reader)
		{
		}

		protected override void LaunchTick(Object state)
		{
			lock (this.launchTimer)
			{
				// do nothing
			}
		}
		
	}
}
