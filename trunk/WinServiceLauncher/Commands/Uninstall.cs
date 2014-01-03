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
	public class Uninstall : sar.Base.Command
	{
		public Uninstall(sar.Base.CommandHub parent) : base(parent, "Uninstall",
		                                                    new List<string> { "uninstall", "u" },
		                                                    @"-u",
		                                                    new List<string> { "-u" })
		{
		}
		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}
			
			Progress.Message = "Uninstalling Service";
			List<string> installArgs = new List<string>();
			
			installArgs.Add(@"/u");
			if (this.commandHub.NoWarning) installArgs.Add(@"/LogToConsole = false");
			if (!this.commandHub.NoWarning) installArgs.Add(@"/LogToConsole = true");

			installArgs.Add(Assembly.GetExecutingAssembly().Location);
			
			ManagedInstallerClass.InstallHelper(installArgs.ToArray());
			
			ConsoleHelper.WriteLine("");
			ConsoleHelper.WriteLine("Uninstall Complete", ConsoleColor.Yellow);

			return ConsoleHelper.EXIT_OK;
		}
	}
}
