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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using skylib.Tools;

namespace WinServiceLauncher
{
	public class WinServiceLauncher : ServiceBase
	{
		public const string MyServiceName = "WinServiceLauncher";
		
		public WinServiceLauncher()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
		}
		
		protected override void Dispose(bool disposing)
		{
			// TODO: Add cleanup code here (if required)
			base.Dispose(disposing);
		}
		
		protected override void OnStart(string[] args)
		{
			ServiceHelper.LogEvent("OnStart");
			Thread thread = new Thread(StartServices);
			thread.Start();
		}
		
		protected override void OnStop()
		{
			ServiceHelper.LogEvent("OnStop");
			// TODO: Add tear-down code here (if required) to stop your service.
		}
		
		private static void StartServices()
		{
			ConsoleHelper.Start(@"C:\Program Files\CCleaner\CCleaner64.exe", "/AUTO");
			ConsoleHelper.Start(@"C:\Program Files (x86)\Plex\Plex Media Server\Plex Media Server.exe");
		}
	}
}
