/* Copyright (C) 2020 Kevin Boronka
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

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WinServiceLauncher
{
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private readonly ServiceProcessInstaller serviceProcessInstaller;
		private readonly ServiceInstaller serviceInstaller;

		public ProjectInstaller() 
			: base()
		{
			serviceProcessInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			Installers.AddRange(new Installer[] { serviceProcessInstaller, serviceInstaller });

			this.BeforeInstall += ProjectInstaller_BeforeInstall;
			this.BeforeUninstall += ProjectInstaller_BeforeUninstall;
		}

		private void SetServiceName()
		{
			string serviceName = GetContextParameter("ServiceName").Trim();
			sar.Base.Program.Log("service name = " + serviceName);
			serviceInstaller.ServiceName = string.IsNullOrEmpty(serviceName) ? WinServiceLauncher.MyServiceName : serviceName;
		}

		private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
		{
			SetServiceName();
		}

		private void ProjectInstaller_BeforeInstall(object sender, InstallEventArgs e)
		{
			string username = GetContextParameter("user").Trim();
			string password = GetContextParameter("password").Trim();

			sar.Base.Program.Log("username = " + username);
			sar.Base.Program.Log("password = " + password);

			if (!string.IsNullOrEmpty(username))
			{
				if (username != "") serviceProcessInstaller.Username = username;
				if (password != "") serviceProcessInstaller.Password = password;

				serviceProcessInstaller.Account = ServiceAccount.User;
			}
			else
			{
				serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
			}

			SetServiceName();


			serviceInstaller.StartType = ServiceStartMode.Automatic;
		}

		public string GetContextParameter(string key)
		{
			string returnValue;
			try
			{
				returnValue = this.Context.Parameters[key].ToString();
			}
			catch
			{
				returnValue = "";
			}

			return returnValue;
		}
	}
}
