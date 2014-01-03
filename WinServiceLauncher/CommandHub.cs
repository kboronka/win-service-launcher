using System;
using System.Collections.Generic;

using sar.Base;
using sar.Commands;
using sar.Tools;
using WinServiceLauncher.Commands;

namespace WinServiceLauncher
{
	public class CommandHub : sar.Base.CommandHub
	{
		private List<Command> allCommands;
		public CommandHub() : base()
		{
			// load all command modules
			this.allCommands = new List<Command>() {
				new Help(this),
				new Kill(this),
				new AppShutdownWait(this),
				new Delay(this),
				new Install(this),
				new Uninstall(this),
				new Run(this),
				new Start(this),
				new Stop(this)
			};
		}
	}
}