/*
 * Created by SharpDevelop.
 * User: Boronka
 * Date: 7/1/2013
 * Time: 9:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using sar.Tools;

namespace WinServiceLauncher
{
	public class Launcher
	{
		private string filename;
		private string arguments;
		private string domain;
		private string username;
		private string password;
		
		public Launcher(string filename, string arguments)
		{
			this.filename = filename;
			this.arguments = arguments;
		}

		public Launcher(string filename, string arguments, string username, string password)
		{
			this.filename = filename;
			this.arguments = arguments;
			this.domain = System.Environment.MachineName;
			this.username = username;
			this.password = password;
		}
		
		public Launcher(string filename, string arguments, string domain, string username, string password)
		{
			this.filename = filename;
			this.arguments = arguments;
			this.domain = domain;
			this.username = username;
			this.password = password;
		}
		
		public void Launch()
		{
			try
			{
				WinServiceLauncher.Log("Launching " + this.filename + this.arguments);
				
				if (String.IsNullOrEmpty(domain))
				{
					ConsoleHelper.Start(this.filename, this.arguments);
				}
				else
				{
					ConsoleHelper.StartAs(this.filename, this.arguments, this.domain, this.username, this.password);
				}
				WinServiceLauncher.Log("Launching complete");
			}
			catch (Exception ex)
			{
				WinServiceLauncher.Log("Launching failed");
				WinServiceLauncher.Log(ex);
			}
		}
	}
}
