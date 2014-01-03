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
	public class Stop : sar.Base.Command
	{
		public Stop(sar.Base.CommandHub parent) : base(parent, "Stop",
		                                              new List<string> { "stop" },
		                                              @"-stop",
		                                              new List<string> { "-stop" })
		{
		}
		
		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}
			
			Progress.Message = "Stopping Service";
			bool success = ServiceHelper.TryStop("WinServiceLauncher.exe");
			
			if (success)
			{
				ConsoleHelper.WriteLine("");
				ConsoleHelper.WriteLine("Service Stopped", ConsoleColor.Yellow);
				return ConsoleHelper.EXIT_OK;
			}
			else
			{
				ConsoleHelper.WriteLine("");
				ConsoleHelper.WriteLine("Service Failed to Stop", ConsoleColor.Red);
				return ConsoleHelper.EXIT_ERROR;
			}
		}
	}
}
