/* Copyright (C) 2013 Kevin Boronka
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
using System.Configuration.Install;
using System.ServiceProcess;

namespace WinServiceLauncher
{
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private ServiceProcessInstaller serviceProcessInstaller;
		private ServiceInstaller serviceInstaller;
		
		public ProjectInstaller()
		{
			//this.AfterInstall += new InstallEventHandler(ServiceInstaller_AfterInstall);
			serviceProcessInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			// Here you can set properties on serviceProcessInstaller or register event handlers

			string username = GetContextParameter("user").Trim();
			string password = GetContextParameter("password").Trim();
			WinServiceLauncher.Log("username = " + username);
			WinServiceLauncher.Log("password = " + password);
			
			if (!String.IsNullOrEmpty(username))
			{
				if (username != "") serviceProcessInstaller.Username = username;
				if (password != "") serviceProcessInstaller.Password = password;
				
				serviceProcessInstaller.Account = ServiceAccount.User;
			}
			else
			{
				serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
				//serviceProcessInstaller.Account = ServiceAccount.NetworkService;
			}
			
			serviceInstaller.ServiceName = WinServiceLauncher.MyServiceName;
			serviceInstaller.StartType = ServiceStartMode.Automatic;
			//serviceInstaller.DelayedAutoStart = true;
			
			this.Installers.AddRange(new Installer[] { serviceProcessInstaller, serviceInstaller });
		}
		
		private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
		{
			//ServiceController serviceController = new ServiceController(serviceInstaller.ServiceName);
			//ServiceHelper.ChangeStartMode(serviceController, ServiceStartMode.Automatic);
			//serviceController.Start();
		}
		
		public string GetContextParameter(string key)
		{
			string sValue = "";
			try
			{
				sValue = this.Context.Parameters[key].ToString();
			}
			catch
			{
				sValue = "";
			}
			return sValue;
		}
	}
}
