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
using System.ServiceProcess;
using System.Threading;
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
				Program.Log(ex);
			}
		}

		public static void StopServices()
		{
			try
			{
				if (Configuration.All.Launchers != null)
				{
					foreach (Launcher app in Configuration.All.Launchers)
					{
						app.Kill();
					}
				}
			}
			catch (Exception ex)
			{
				Program.Log(ex);
			}
		}
	}
}