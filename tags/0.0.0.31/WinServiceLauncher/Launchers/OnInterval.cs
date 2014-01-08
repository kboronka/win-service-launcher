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
	public class OnInterval : Schedule
	{
		private long interval;		//ms
		
		public OnInterval(Launcher parent, long interval) : base(parent)
		{
			this.interval = interval;
		}
		
		public OnInterval(Launcher parent, XML.Reader reader) : base(parent, reader)
		{
			this.interval = reader.GetAttributeLong("interval");
		}
		
		protected override void LaunchTick(Object state)
		{
			lock (this.launchTimer)
			{
				try
				{
					if (DateTime.Now > this.lastRun.AddMilliseconds(this.interval))
					{
						this.Launch();
					}
				}
				catch
				{

				}
				finally
				{
					if (this.interval > 0)
					{
						this.launchTimer.Change(this.interval, Timeout.Infinite);
					}
				}
			}
		}
	}
}
