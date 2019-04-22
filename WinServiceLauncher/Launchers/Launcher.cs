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
		private string workingPath;
		private string command;
		private string arguments;
		private List<Schedule> schedules;
		
		public Launcher(XML.Reader reader)
		{
			this.name = reader.GetAttributeString("name");
			this.workingPath = reader.GetAttributeString("working-path");
			this.command = reader.GetAttributeString("command");
			this.arguments = reader.GetAttributeString("arguments");
			this.schedules = new List<Schedule>();
			
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement))
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
						case "KeepAlive":
							this.schedules.Add(new KeepAlive(this, reader));
							break;
					}
				}
			}
		}

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(name)) name = command;
				return name;
			}
		}

		public string WorkingPath { get { return workingPath; } }

		public string Command { get { return command; } }

		public string Arguments { get { return arguments; } }
		
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
			
			Program.Log("Shutting Down " + this.command);
			ConsoleHelper.KillProcess(this.command);
		}
		
		public void Serialize(XML.Writer writer)
		{
			writer.WriteStartElement("Launcher");
			writer.WriteAttributeString("name", this.Name);
			writer.WriteAttributeString("workingPath", this.workingPath);
			writer.WriteAttributeString("command", this.workingPath);
			writer.WriteAttributeString("arguments", this.Arguments);
			
			foreach (Schedule schedule in this.schedules)
			{
				schedule.Serialize(writer);
			}
			
			writer.WriteEndElement();	// Launcher
		}
		
		#endregion
		
	}
}
