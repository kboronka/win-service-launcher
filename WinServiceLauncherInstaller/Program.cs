using System;
using System.Collections.Generic;
using System.IO;

using sar.Tools;

namespace WinServiceLauncherInstaller
{
	class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				ConsoleHelper.Start();
				ConsoleHelper.ApplicationTitle();
				
				string serviceEXE = IO.FindFile("WinServiceLauncher.exe");
				string serviceFilename = IO.GetFilename(serviceEXE);
				string serviceName = StringHelper.TrimEnd(serviceFilename, IO.GetFileExtension(serviceEXE).Length + 1);
				string serviceRoot = IO.GetRoot(serviceEXE);
				
				ServiceHelper.TryStop(serviceEXE);
				ServiceHelper.TryUninstall(serviceEXE);
				
				
				ServiceHelper.Install("4.0", serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " installed");
				
				
				ServiceHelper.Start(serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " started");
				ConsoleHelper.Write(serviceName + " should be installed and running", ConsoleColor.Yellow);

			}
			catch (Exception ex)
			{
				ServiceHelper.TryStop("WinServiceLauncher");
				ServiceHelper.TryUninstall("WinServiceLauncher");
				ConsoleHelper.WriteException(ex);
				ConsoleHelper.ReadKey();
			}

			ConsoleHelper.Shutdown();
		}
	}
}