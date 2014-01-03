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
	public class Start : sar.Base.Command
	{
		public Start(sar.Base.CommandHub parent) : base(parent, "Start",
		                                                new List<string> { "start" },
		                                                @"-start",
		                                                new List<string> { "-start" })
		{
		}
		
		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}
			
			Progress.Message = "Starting Service";
			bool success = ServiceHelper.TryStart("WinServiceLauncher.exe");
			
			if (success)
			{
				ConsoleHelper.WriteLine("Service Started", ConsoleColor.Yellow);
				return ConsoleHelper.EXIT_OK;
			}
			else
			{
				ConsoleHelper.WriteLine("Service Failed to Start", ConsoleColor.Red);
				return ConsoleHelper.EXIT_ERROR;
			}
		}
	}
}
