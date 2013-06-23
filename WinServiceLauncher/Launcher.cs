/*
 * Created by SharpDevelop.
 * User: Boronka
 * Date: 7/1/2013
 * Time: 9:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using sar.Tools;

namespace WinServiceLauncher
{
	public class Launcher
	{
		private string name;
		private string filename;
		private string arguments;
		private string domain;
		private string username;
		private string password;
		
		#region constructors
		
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
		
		public Launcher(XmlReader reader)
		{
			this.name = reader.GetAttribute("name");
			this.filename = reader.GetAttribute("filename");
			this.arguments = reader.GetAttribute("arguments");
			this.domain = reader.GetAttribute("domain");
			this.username = reader.GetAttribute("username");
			this.password = reader.GetAttribute("password");
		}
		
		#endregion
		
		#region methods
		
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
		
		public void Serialize(XmlWriter writer)
		{
			if (String.IsNullOrEmpty(this.name)) this.name = "";
			if (String.IsNullOrEmpty(this.filename)) this.filename = "";
			if (String.IsNullOrEmpty(this.arguments)) this.arguments = "";
			if (String.IsNullOrEmpty(this.domain)) this.domain = "";
			if (String.IsNullOrEmpty(this.username)) this.username = "";
			if (String.IsNullOrEmpty(this.password)) this.password = "";
						
			writer.WriteStartElement("Launcher");
			writer.WriteAttributeString("name", this.name);
			writer.WriteAttributeString("filename", this.filename);
			writer.WriteAttributeString("arguments", this.arguments);
			writer.WriteAttributeString("domain", this.domain);
			writer.WriteAttributeString("username", this.username);
			writer.WriteAttributeString("password", this.password);
			writer.WriteEndElement();	// Launcher
		}
		
		#endregion
		
	}
}
