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

using sar.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;

namespace WinServiceLauncher.Launchers
{
	public abstract class Schedule
	{
		protected Launcher parent;
		protected DateTime lastRun;
		protected int loopRate = 100;
		protected int processID;
		protected string processName;
		private List<int> processIDs;

		public Schedule(Launcher parent)
		{
			this.parent = parent;
			this.processIDs = new List<int>();
		}

		public Schedule(Launcher parent, XML.Reader reader)
		{
			this.parent = parent;
			this.lastRun = reader.GetAttributeTimestamp("lastrun");
			this.processIDs = new List<int>();
		}

		internal abstract void Serialize(XML.Writer writer);

		#region Async

		private Thread launchLoopThread;
		private bool launchLoopShutdown;
		private Process spawnedProcess;

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
				if (spawnedProcess != null)
				{
					KillAllProcesses(this.processIDs);
				}

				this.launchLoopShutdown = true;
				if (this.launchLoopThread != null)
				{
					this.launchLoopThread.Join();
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
					if (spawnedProcess != null && this.processIDs.Count == 0)
					{
						Thread.Sleep(8000);
						this.processIDs = LogProcessIDs(spawnedProcess.Id);
					}
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

		private static void KillAllProcesses(List<int> pids)
		{
			foreach (var pid in pids)
			{
				ConsoleHelper.WriteLine(String.Format("Killing PID: {0}", pid));
				Program.Log(String.Format("Killing PID: {0}", pid));
				try
				{
					Process proc = Process.GetProcessById(pid);
					if (!proc.HasExited)
					{
						proc.Kill();
					}
				}
				catch (ArgumentException)
				{
					// Process already exited.
				}
			}
		}

		private static List<int> LogProcessIDs(int pid)
		{
			var processSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
			ManagementObjectCollection processCollection = processSearcher.Get();

			var pids = new List<int>();
			if (processCollection != null)
			{
				foreach (ManagementObject mo in processCollection.OfType<ManagementObject>())
				{
					pids.AddRange(LogProcessIDs(Convert.ToInt32(mo["ProcessID"])));
				}
			}

			pids.Add(pid);
			return pids;
		}

		#endregion Async
	}
}