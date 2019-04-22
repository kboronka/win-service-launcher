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
using System.Diagnostics;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public class KeepAlive : Schedule
	{
		public KeepAlive(Launcher parent) : base(parent)
		{
			this.processName = parent.Command;
		}
		
		public KeepAlive(Launcher parent, XML.Reader reader) : base(parent, reader)
		{
			this.processName = reader.GetAttributeString("processName");
		}
		
		protected override void ServiceLauncher()
		{
			Process process = null;
			
			if (processID > 0)
			{
				try
				{
					process = Process.GetProcessById(this.processID);
					if (processName != process.ProcessName)
					{
						processName = process.ProcessName;
					}
				}
				catch
				{

				}
				
				if (process == null) this.processID = -1;
			}
			else
			{
				var processList = Process.GetProcessesByName(this.processName);
				if (processList.Length > 0)
				{
					process = processList[0];
					processID = process.Id;
				}
			}
			
			if (process == null)
			{
				this.Launch();
				Thread.Sleep(1000);
			}
		}
		
		internal override void Serialize(XML.Writer writer)
		{
			writer.WriteStartElement("KeepAlive");
			writer.WriteAttributeString("processName", this.processName);
			writer.WriteEndElement();	// KeepAlive
		}
	}
}
