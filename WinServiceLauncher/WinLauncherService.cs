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

using sar.Tools;

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
			WinServiceLauncher.Log("OnStart");
			Thread thread = new Thread(StartServices);
			thread.Start();
		}
		
		protected override void OnStop()
		{
			WinServiceLauncher.Log("OnStop");
			// TODO: Add tear-down code here (if required) to stop your service.
		}
		
		private void StartServices()
		{
			try
			{
				WinServiceLauncher.Log(ConsoleHelper.HR);
				WinServiceLauncher.Log("StartServices()");
				WinServiceLauncher.Log("Environment.UserInteractive = " + Environment.UserInteractive.ToString());
				WinServiceLauncher.Log("Username = " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
				
				Configuration.Load();
				
				foreach (Launcher app in Configuration.All.Launchers)
				{
					app.LaunchAsync();
				}
			}
			catch (Exception ex)
			{
				WinServiceLauncher.Log(ex);
			}
		}
		
		#region logger
		private static FileLogger logger;

		public static void Log(Exception ex)
		{
			WinServiceLauncher.Log(ex.Message);
			WinServiceLauncher.Log(ex.StackTrace);
		}
		
		public static void Log(string message)
		{
			try
			{
				ServiceHelper.LogEvent(message);
			}
			catch
			{
				
			}
			
			try
			{
				if (WinServiceLauncher.logger == null)
				{
					WinServiceLauncher.logger = new FileLogger("debug.log");
				}
				
				WinServiceLauncher.logger.WriteLine(message);
			}
			catch
			{
				
			}
		}
		
		#endregion
	}
}
