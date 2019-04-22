/* Copyright (C) 2019 Kevin Boronka
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using sar.Tools;
using WinServiceLauncher.Launchers;

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
				if (Configuration.all == null)
				{
					var path = ApplicationInfo.CurrentDirectory + AssemblyInfo.Name + ".xml";
					#if DEBUG
					path = Path.Combine(ApplicationInfo.CurrentDirectory, @"..\..\..\");
					path = Path.Combine(path, AssemblyInfo.Name + ".xml");
					#endif
					
					Configuration.all = new Configuration(path);
					all.Save();
				}
				
				return Configuration.all;
			}
		}
		
		public static void Load()
		{
			Configuration.all = Configuration.All;
		}
		
		#endregion
		
		private List<Launcher> launchers;
		
		public 	Configuration(string path)
			: base(path)
		{
			
		}
		
		public List<Launcher> Launchers
		{
			get
			{
				if (launchers == null) Configuration.Load();
				return launchers;
			}
		}
		
		protected override void Deserialize(XML.Reader reader)
		{
			this.launchers = new List<Launcher>();

			try
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case "Launcher":
								this.launchers.Add(new Launcher(reader));
								break;
						}
					}
				}
			}
			catch
			{
				
			}
		}
		
		protected override void Serialize(XML.Writer writer)
		{
			writer.WriteStartElement("Launchers");

			foreach (Launcher launcher in Configuration.All.Launchers)
			{
				launcher.Serialize(writer);
			}
			
			writer.WriteEndElement();	// Launchers
		}
	}
}
