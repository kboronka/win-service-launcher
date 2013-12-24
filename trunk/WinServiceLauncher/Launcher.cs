/* Copyright (C) 2013 Kevin Boronka
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
		private long interval;
		
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
		
		public Launcher(XML.Reader reader)
		{
			this.name = reader.GetAttributeString("name");
			this.filename = reader.GetAttributeString("filename");
			this.arguments = reader.GetAttributeString("arguments");
			this.domain = reader.GetAttributeString("domain");
			this.username = reader.GetAttributeString("username");
			this.password = reader.GetAttributeString("password");
			this.interval = reader.GetAttributeLong("interval");
		}
		
		#endregion
		
		#region methods
		
		#region Async
		
		public void LaunchAsync()
		{
			this.launchTimer = new Timer(this.LaunchTick, null, 1000, Timeout.Infinite);
		}

		private System.Threading.Timer launchTimer;
		
		private void LaunchTick(Object state)
		{
			try
			{
				this.Launch();
			}
			catch
			{

			}
			finally
			{
				if (this.interval > 0)
				{
					this.launchTimer.Change(this.interval, Timeout.Infinite );
				}
			}
		}
		
		#endregion
		
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
		
		public void Serialize(XML.Writer writer)
		{
			writer.WriteStartElement("Launcher");
			writer.WriteAttributeString("name", this.name);
			writer.WriteAttributeString("filename", this.filename);
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
