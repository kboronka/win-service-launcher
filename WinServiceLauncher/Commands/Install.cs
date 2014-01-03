
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
	public class Install : sar.Base.Command
	{
		public Install(sar.Base.CommandHub parent) : base(parent, "Install",
		                                                  new List<string> { "install", "i" },
		                                                  @"-i",
		                                                  new List<string> { "-i" })
		{
		}
		
		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}
			
			Progress.Message = "Installing Service";
			List<string> installArgs = new List<string>();
			
			
			if (this.commandHub.NoWarning) installArgs.Add(@"/LogToConsole = false");
			if (!this.commandHub.NoWarning) installArgs.Add(@"/LogToConsole = true");

			installArgs.Add(Assembly.GetExecutingAssembly().Location);
			
			ManagedInstallerClass.InstallHelper(installArgs.ToArray());
			
			ConsoleHelper.WriteLine("");
			ConsoleHelper.WriteLine("Install Complete", ConsoleColor.Yellow);

			return ConsoleHelper.EXIT_OK;
		}
	}
}
