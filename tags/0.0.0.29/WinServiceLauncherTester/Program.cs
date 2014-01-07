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
using System.IO;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncherTester
{
	class Program
	{
		public static void Main(string[] args)
		{
			Progress progressBar = new Progress();
			Thread backgroundThread = new Thread(new ThreadStart(progressBar.Enable));
			
			try
			{
				ConsoleHelper.Start();
				ConsoleHelper.ApplicationTitle();
				
				ConsoleHelper.WriteLine("Environment.UserInteractive = " + Environment.UserInteractive.ToString());
				ConsoleHelper.WriteLine("Username = " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);

				//return;
				ConsoleHelper.KillProcess("PlexDlnaServer.exe");
				ConsoleHelper.KillProcess("PlexScriptHost.exe");
				ConsoleHelper.KillProcess("Plex Media Server.exe");
				ConsoleHelper.KillProcess("PlexDlnaServer.exe");

				// rename service exe file
				string serviceEXE = IO.FindFile("WinServiceLauncher.exe");
				string serviceFilename = IO.GetFilename(serviceEXE);
				string serviceName = StringHelper.TrimEnd(serviceFilename, IO.GetFileExtension(serviceEXE).Length + 1);
				string serviceRoot = IO.GetRoot(serviceEXE);
				
				ServiceHelper.TryStop(serviceEXE);
				ServiceHelper.TryUninstall(serviceEXE);
				
				//string username = ConsoleHelper.Input("Username: ");
				//string password = ConsoleHelper.Input("Password: ");
				
				ServiceHelper.Install("4.0", serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " installed");
				
				
				ServiceHelper.Start(serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " started");
				ConsoleHelper.Write("done - press any key to stop and uninstall the service", ConsoleColor.Yellow);
				ConsoleHelper.ReadKey();
				ConsoleHelper.WriteLine();
				
				
				ServiceHelper.TryStop(serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " stopped");
				
				
				ServiceHelper.Uninstall("4.0", serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " uninstalled");
				ConsoleHelper.Write("done - press any key to exit", ConsoleColor.Yellow);
				ConsoleHelper.ReadKey();
				ConsoleHelper.WriteLine();
			}
			catch (Exception ex)
			{
				Progress.UpdateTimer.Enabled = false;
				backgroundThread.Abort();

				ServiceHelper.TryStop("WinServiceLauncher");
				ServiceHelper.TryUninstall("WinServiceLauncher");
				ConsoleHelper.WriteException(ex);
				ConsoleHelper.ReadKey();
			}

			Progress.UpdateTimer.Enabled = false;
			backgroundThread.Abort();
			ConsoleHelper.Shutdown();
		}
	}
}