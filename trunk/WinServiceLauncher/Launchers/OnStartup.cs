
using System;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public class OnStartup : Schedule
	{
		public OnStartup(Launcher parent) : base(parent)
		{
		}
		
		public OnStartup(Launcher parent, XML.Reader reader) : base(parent, reader)
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
