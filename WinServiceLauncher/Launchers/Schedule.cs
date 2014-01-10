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
using System.Threading;

using sar.Tools;

namespace WinServiceLauncher.Launchers
{
	public abstract class Schedule
	{
		protected Launcher parent;
		protected DateTime lastRun;
		protected int loopRate = 100;
		
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
				this.launchLoopThread.Join();
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
		
		public void Launch()
		{
			try
			{
				
				Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Launching " + this.parent.Filename + this.parent.Arguments);
				
				if (String.IsNullOrEmpty(this.parent.Domain))
				{
					ConsoleHelper.Start(this.parent.Filepath, this.parent.Arguments);
				}
				else
				{
					ConsoleHelper.StartAs(this.parent.Filepath, this.parent.Arguments, this.parent.Domain, this.parent.Username, this.parent.Password);
				}
				
				Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Launching complete");
				this.lastRun = DateTime.Now;
			}
			catch (Exception ex)
			{
				Program.Log(this.parent.Name + " - " + this.GetType().Name.ToString() + " Launching failed");
				Program.Log(ex);
			}
		}
		
		#endregion
		
	}
}
