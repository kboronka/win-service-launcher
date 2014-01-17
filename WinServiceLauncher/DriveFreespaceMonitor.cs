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
using System.IO;

using sar.Tools;
using sar.Socket;

namespace WinServiceLauncher
{
	public class DriveFreespaceMonitor : sar.Socket.SocketClient
	{
		private string machineName;
		
		public DriveFreespaceMonitor() : base("localhost", 8111, Program.ErrorLog, Program.DebugLog)
		{
			this.machineName = System.Environment.MachineName;
		}
		
		public DriveFreespaceMonitor(XML.Reader reader): base(reader.GetAttributeString("host"), int.Parse(reader.GetAttributeString("port")), Program.ErrorLog, Program.DebugLog)
		{
			this.machineName = reader.GetAttributeString("MachineName");
			
			if (String.IsNullOrEmpty(this.machineName))
			{
				this.machineName = System.Environment.MachineName;
			}
		}
		
		private void ReportFreeSpace()
		{
			foreach (DriveInfo drive in DriveInfo.GetDrives())
			{
				if (drive.IsReady && drive.DriveType == DriveType.Fixed)
				{
					string name = drive.Name;
					//drive.TotalFreeSpace;
				}
			}
		}
	}
}
