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
		
		#region Async
		
		protected System.Threading.Timer launchTimer;

		public void StartAsync()
		{
			lock (this.launchTimer)
			{
				this.launchTimer = new Timer(this.LaunchTick, null, 5000, Timeout.Infinite);
			}
		}
		
		public void StopAsync()
		{
			lock (this.launchTimer)
			{
				this.launchTimer.Dispose();
				this.launchTimer = null;
			}
		}

		protected abstract void LaunchTick(Object state);
		
		public void Launch()
		{
			try
			{
				Program.Log("Launching " + this.parent.Filename + this.parent.Arguments);
				
				if (String.IsNullOrEmpty(this.parent.Domain))
				{
					ConsoleHelper.Start(this.parent.Filepath, this.parent.Arguments);
				}
				else
				{
					ConsoleHelper.StartAs(this.parent.Filepath, this.parent.Arguments, this.parent.Domain, this.parent.Username, this.parent.Password);
				}
				
				Program.Log("Launching complete");
			}
			catch (Exception ex)
			{
				Program.Log("Launching failed");
				Program.Log(ex);
			}
		}
		
		#endregion
		
	}
}
