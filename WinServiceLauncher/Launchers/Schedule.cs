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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Management;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public abstract class Schedule
	{
		protected Launcher parent;
		protected DateTime lastRun;
		protected int loopRate = 100;
		protected int processID;
		protected string processName;
		
		public Schedule(Launcher parent)
		{
			this.parent = parent;
		}
		
		public Schedule(Launcher parent, XML.Reader reader)
		{
			this.parent = parent;
			this.lastRun = reader.GetAttributeTimestamp("lastrun");
		}
		
		internal abstract void Serialize(XML.Writer writer);
		
		#region Async
		
		private Thread launchLoopThread;
		private bool launchLoopShutdown;
		Process spawnedProcess;

		public void StartAsync()
		{
			try
			{
				this.launchLoopThread = new Thread(this.LaunchLoop);
				this.launchLoopThread.Start();
			}
			catch (Exception ex)
			{
				Program.Log(ex);
			}
		}
		
		public void StopAsync()
		{
			try
			{
				this.launchLoopShutdown = true;
				
				if (this.launchLoopThread != null) {
					this.launchLoopThread.Join();
				}
				
				if (spawnedProcess != null && !spawnedProcess.HasExited)
				{
					KillProcessAndChildrens(spawnedProcess.Id);
				}
			}
			catch (Exception ex)
			{
				Program.Log(ex);
			}
		}
		
		private void LaunchLoop()
		{
			Thread.Sleep(250);
			Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Loop Started");
			
			while (!launchLoopShutdown)
			{
				try
				{
					this.ServiceLauncher();
					Thread.Sleep(100);
				}
				catch (Exception ex)
				{
					Program.Log(ex);
					Thread.Sleep(1000);
				}
			}
			
			Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Loop Shutdown Gracefully");
		}
		
		protected abstract void ServiceLauncher();
		
		public Process Launch()
		{
			try
			{
				Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Launching " + this.parent.Command + " " + this.parent.Arguments);
				
				spawnedProcess = Start(parent.WorkingPath, parent.Command, parent.Arguments, parent.EnvironmentVariables);
				this.processID = spawnedProcess.Id;
				this.processName = spawnedProcess.ProcessName;
				
				Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Launching complete");
				this.lastRun = DateTime.Now;
			}
			catch (Exception ex)
			{
				Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Launching failed");
				Program.Log(ex);
			}
			
			return spawnedProcess;
		}
		
		public static Process Start(string workingDirectory, string command, string arguments, List<EnvironmentVariable> variables)
		{
			arguments = arguments.TrimWhiteSpace();
			var startInfo = new ProcessStartInfo();

			foreach (DictionaryEntry e in System.Environment.GetEnvironmentVariables())
			{
				startInfo.EnvironmentVariables[e.Key.ToString()] = e.Value.ToString();
			}
			
			foreach (var variable in variables)
			{
				startInfo.EnvironmentVariables[variable.Variable] = variable.Value;
			}

			startInfo.WorkingDirectory = workingDirectory;
			startInfo.FileName = command;
			startInfo.Arguments = arguments;
			startInfo.UseShellExecute = false;
			
			return Process.Start(startInfo);
		}
		
		private static void KillProcessAndChildrens(int pid)
		{
			var processSearcher = new ManagementObjectSearcher
				("Select * From Win32_Process Where ParentProcessID=" + pid);
			ManagementObjectCollection processCollection = processSearcher.Get();

			try
			{
				Process proc = Process.GetProcessById(pid);
				if (!proc.HasExited) proc.Kill();
			}
			catch (ArgumentException)
			{
				// Process already exited.
			}

			if (processCollection != null)
			{
				foreach (ManagementObject mo in processCollection)
				{
					KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
				}
			}
		}
		
		#endregion
		
	}
}
