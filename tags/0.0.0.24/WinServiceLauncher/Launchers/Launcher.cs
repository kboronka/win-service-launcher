/* Copyright (C) 2014 Kevin Boronka
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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public class Launcher
	{
		private string name;
		private string filepath;
		private string filename;
		private string arguments;
		private string domain;
		private string username;
		private string password;
		private long interval;

		private List<Schedule> schedules;
		
		#region properties

		public string Name {
			get { return name; }
		}

		public string Filepath {
			get { return filepath; }
		}

		public string Filename {
			get { return filename; }
		}

		public string Arguments {
			get { return arguments; }
		}

		public string Domain {
			get { return domain; }
		}

		public string Username {
			get { return username; }
		}

		public string Password {
			get { return password; }
		}
		
		#endregion
		
		#region constructors
		
		public Launcher(string filepath, string arguments)
		{
			this.filepath = filepath;
			this.filename = IO.GetFilename(this.filepath);
			this.arguments = arguments;
		}

		public Launcher(string filepath, string arguments, string username, string password)
		{
			this.filepath = filepath;
			this.filename = IO.GetFilename(this.filepath);
			this.arguments = arguments;
			this.domain = System.Environment.MachineName;
			this.username = username;
			this.password = password;
		}
		
		public Launcher(string filename, string arguments, string domain, string username, string password)
		{
			this.filepath = filename;
			this.filename = IO.GetFilename(this.filepath);
			this.arguments = arguments;
			this.domain = domain;
			this.username = username;
			this.password = password;
		}
		
		public Launcher(XML.Reader reader)
		{
			this.name = reader.GetAttributeString("name");
			this.filepath = reader.GetAttributeString("filename");
			this.filename = IO.GetFilename(this.filepath);
			this.arguments = reader.GetAttributeString("arguments");
			this.domain = reader.GetAttributeString("domain");
			this.username = reader.GetAttributeString("username");
			this.password = reader.GetAttributeString("password");
			this.interval = reader.GetAttributeLong("interval");
			
			
			return;
			
			// code is incomplete
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					// TODO: handled </Laucher>
					switch (reader.Name)
					{
						case "OnInterval":
							this.schedules.Add(new OnInterval(this, reader));
							break;
						case "OnTimeOfDay":
							this.schedules.Add(new OnTimeOfDay(this, reader));
							break;
						case "OnStartup":
							this.schedules.Add(new OnStartup(this, reader));
							break;
						case "OnShutdown":
							this.schedules.Add(new OnShutdown(this, reader));
							break;
					}
				}
			}
		}
		
		#endregion
		
		#region methods
		
		public void Start()
		{
			foreach (Schedule schedule in schedules)
			{
				schedule.StartAsync();
			}
		}
		
		public void Kill()
		{
			foreach (Schedule schedule in schedules)
			{
				schedule.StopAsync();
			}
			
			WinServiceLauncher.Log("Shutting Down " + this.filename);
			ConsoleHelper.KillProcess(this.filename);
		}
		
		public void Serialize(XML.Writer writer)
		{
			writer.WriteStartElement("Launcher");
			writer.WriteAttributeString("name", this.name);
			writer.WriteAttributeString("filename", this.filepath);
			writer.WriteAttributeString("arguments", this.arguments);
			writer.WriteAttributeString("domain", this.domain);
			writer.WriteAttributeString("username", this.username);
			writer.WriteAttributeString("password", this.password);
			writer.WriteAttributeString("interval", this.interval.ToString());
			writer.WriteEndElement();	// Launcher
		}
		
		#endregion
		
	}
}
