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
	public static class Configuration
	{
		private static List<Launcher> launchers;
		private static string path;
		private static SocketServer socketServer;
		
		public static List<Launcher> Launchers
		{
			get
			{
				if (launchers == null)
				{
					Load(ApplicationInfo.CommonDataDirectory + "WinServiceLauncher.xml");
				}
				
				return launchers;
			}
		}

		public static void Save()
		{
			Configuration.Save(Configuration.path);
		}
		
		public static void Save(string path)
		{
			Configuration.path = path;
			XmlWriter writer = XmlWriter.Create(path, WriterSettings());
			Configuration.Serialize(writer);
			writer.Close();
		}
		
		public static void Load(string path)
		{
			Configuration.path = path;
			
			if (!File.Exists(path))
			{
				Configuration.launchers = new List<Launcher>();
			}
			else
			{
				XmlReader reader = XmlReader.Create(path, ReaderSettings());
				Configuration.Deserialize(reader);
				reader.Close();
			}
		}
		
		private static void Deserialize(XmlReader reader)
		{
			Configuration.launchers = new List<Launcher>();

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					switch (reader.Name)
					{
						case "Launcher":
							Configuration.launchers.Add(new Launcher(reader));
							break;
						case "SocketServer":
							socketServer = new SocketServer(reader);
							break;
					}
				}
			}
		}
		
		private static void Serialize(XmlWriter writer)
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("WinServiceLauncher");
			writer.WriteAttributeString("version", "1.0.0.0");

			if (Configuration.socketServer != null) Configuration.socketServer.Serialize(writer);

			writer.WriteStartElement("Launchers");

			foreach (Launcher launcher in Configuration.Launchers)
			{
				launcher.Serialize(writer);
			}
						
			writer.WriteEndElement();	// Launchers
			writer.WriteEndElement();	// WinServiceLauncher
		}
		
		private static XmlReaderSettings ReaderSettings()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.CloseInput = true;
			settings.IgnoreComments = true;
			settings.IgnoreProcessingInstructions = true;
			settings.IgnoreWhitespace = true;
			return settings;
		}

		private static XmlWriterSettings WriterSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CloseOutput = true;
			settings.Encoding = Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";
			return settings;
		}
	}
}
