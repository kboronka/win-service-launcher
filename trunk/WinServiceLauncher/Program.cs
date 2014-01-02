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
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher
{
	static class Program
	{
		static void Main(string[] args)
		{
			try
			{
				if (System.Environment.UserInteractive)
				{
					Progress progressBar = new Progress();
					Thread backgroundThread = new Thread(new ThreadStart(progressBar.Enable));
					
					ConsoleHelper.Start();
					ConsoleHelper.ApplicationTitle();

					try
					{
						string parameter = string.Concat(args);
						switch (parameter)
						{
							case "-i":
								Progress.Message = "Installing Service";
								ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
								break;
							case "-u":
								Progress.Message = "Uninstall Service";
								ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
								break;
							default:
								Progress.Message = "Stopping Service";
								ServiceHelper.TryStop("WinServiceLauncher");
								Progress.Message = "Installing Service";
								ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
								break;
						}
					}
					catch (Exception ex)
					{
						Progress.UpdateTimer.Enabled = false;
						backgroundThread.Abort();
						
						ServiceHelper.TryStop("WinServiceLauncher");
						ConsoleHelper.WriteException(ex);
						ConsoleHelper.ReadKey();
					}
					
					Progress.UpdateTimer.Enabled = false;
					backgroundThread.Abort();
					ConsoleHelper.Shutdown();
				}
				else
				{
					ServiceBase.Run(new ServiceBase[] { new WinServiceLauncher() });
				}
			}
			catch
			{
				
			}
		}
		
		/*
		public static void Uninstall()
		{
			try
			{
				Progress.Message = "Stopping Service";
				ServiceHelper.TryStop("WinServiceLauncher");

				Progress.Message = "Uninstall Service";
				ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteException(ex);
			}
		}
		*/
	}
}
