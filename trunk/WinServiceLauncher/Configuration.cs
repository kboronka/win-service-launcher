/*
 * Created by SharpDevelop.
 * User: Boronka
 * Date: 7/1/2013
 * Time: 9:46 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using sar.Tools;

namespace WinServiceLauncher
{
	public static class Configuration
	{
		private static List<Launcher> launchers;
		
		public static List<Launcher> Launchers
		{
			get
			{
				if (launchers == null)
				{
					ReadConfiguration(ApplicationInfo.CommonDataDirectory + "WinServiceLauncher.xml");
				}
				
				return launchers;
			}
		}
		
		private static void ReadConfiguration(string path)
		{
			launchers = new List<Launcher>();
			launchers.Add(new Launcher(@"C:\Program Files\CCleaner\CCleaner64.exe", "/AUTO", "Boronka", "test123"));
			launchers.Add(new Launcher(@"C:\Program Files (x86)\Plex\Plex Media Server\Plex Media Server.exe", "", "Boronka", "test123"));			
		}
	}
}
