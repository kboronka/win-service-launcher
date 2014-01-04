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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using sar.Tools;
using WinServiceLauncher.Launchers;

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
			base.Dispose(disposing);
		}
		
		protected override void OnStart(string[] args)
		{
			WinServiceLauncher.Log("OnStart");
			WinServiceLauncher.Log(ConsoleHelper.HR);
			WinServiceLauncher.Log("StartServices()");
			WinServiceLauncher.Log("Environment.UserInteractive = " + Environment.UserInteractive.ToString());
			WinServiceLauncher.Log("Username = " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
			Thread thread = new Thread(StartServices);
			thread.Start();
		}
		
		protected override void OnStop()
		{
			foreach (Launcher app in Configuration.All.Launchers)
			{
				app.Kill();
			}
		}
		
		public static void StartServices()
		{
			try
			{
				Configuration.Load();
				
				if (Configuration.All.Launchers != null)
				{
					foreach (Launcher app in Configuration.All.Launchers)
					{
						app.Start();
					}
				}
			}
			catch (Exception ex)
			{
				WinServiceLauncher.Log(ex);
			}
		}
		
		#region logger
		private static FileLogger debugLog;
		private static FileLogger errorLog;

		public static FileLogger ErrorLog
		{
			get
			{
				if (WinServiceLauncher.errorLog == null)
				{
					WinServiceLauncher.errorLog = new FileLogger("error.log");
				}
				
				return WinServiceLauncher.errorLog;
			}
		}
		
		public static FileLogger DebugLog
		{
			get
			{
				if (WinServiceLauncher.debugLog == null)
				{
					WinServiceLauncher.debugLog = new FileLogger("debug.log");
				}
				
				return WinServiceLauncher.debugLog;
			}
		}
		
		public static void Log(Exception ex)
		{
			try
			{
				WinServiceLauncher.DebugLog.WriteLine("Error: " + ex.Message);
				WinServiceLauncher.ErrorLog.WriteLine(ex.Message);
				WinServiceLauncher.ErrorLog.WriteLine(ex.StackTrace);
			}
			catch
			{
				
			}
		}
		
		public static void Log(string message)
		{
			try
			{
				WinServiceLauncher.DebugLog.WriteLine(message);
			}
			catch
			{
				try
				{
					ServiceHelper.LogEvent(message);
				}
				catch
				{
					
				}
			}
		}
		
		#endregion
	}
}
