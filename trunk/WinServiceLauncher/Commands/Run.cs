using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Threading;
using System.Reflection;
using System.ServiceProcess;

using sar.Base;
using sar.Tools;

namespace WinServiceLauncher.Commands
{
	public class Run : sar.Base.Command
	{
		public Run(sar.Base.CommandHub parent) : base(parent, "Run",
		                                              new List<string> { "run", "r" },
		                                              @"-r",
		                                              new List<string> { "-r" })
		{
		}
		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}
			
			Progress.Message = "Service running in console mode";
			Thread thread = new Thread(WinServiceLauncher.StartServices);
			thread.Start();
			
			while (true)
			{
				Thread.Sleep(10000);
			}
		}
	}
}
