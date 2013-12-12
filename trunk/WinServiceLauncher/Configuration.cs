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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using sar.Tools;

namespace WinServiceLauncher
{
	public class Configuration : sar.Base.Configuration
	{
		#region singleton
		
		private static Configuration all;
		
		public static Configuration All
		{
			get
			{
				if (Configuration.all == null) Configuration.all = new Configuration();				
				return Configuration.all;
			}
		}
		
		public static void Load()
		{
			Configuration.all = Configuration.All;
			all.Save();
		}
		
		#endregion
		
		private List<Launcher> launchers;
		private SocketServer socketServer;
		
		public List<Launcher> Launchers
		{
			get
			{
				if (launchers == null) Configuration.Load();				
				return launchers;
			}
		}
		
		protected override void Deserialize(XmlReader reader)
		{
			this.launchers = new List<Launcher>();

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					switch (reader.Name)
					{
						case "Launcher":
							this.launchers.Add(new Launcher(reader));
							break;
						case "SocketServer":
							socketServer = new SocketServer(reader);
							break;
					}
				}
			}
		}
		
		protected override void Serialize(XmlWriter writer)
		{
			if (this.socketServer != null) this.socketServer.Serialize(writer);

			writer.WriteStartElement("Launchers");

			foreach (Launcher launcher in Configuration.All.Launchers)
			{
				launcher.Serialize(writer);
			}
						
			writer.WriteEndElement();	// Launchers
		}
	}
}
