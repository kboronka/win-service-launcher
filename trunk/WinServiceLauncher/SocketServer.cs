using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using sar.Tools;

namespace WinServiceLauncher
{
	public class SocketServer : sar.Socket.SocketServer
	{
	
		#region constructors
		
		public SocketServer(XmlReader reader): base(int.Parse(reader.GetAttribute("port")), Encoding.ASCII)
		{
		
		}
		
		#endregion
		
		#region methods
				
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("SocketServer");
			writer.WriteAttributeString("port", base.port.ToString());
			writer.WriteEndElement();	// Launcher
		}
		
		#endregion
		
	}
}
