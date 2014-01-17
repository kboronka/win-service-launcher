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
using System.Threading;
using System.Reflection;
using System.ServiceProcess;

using sar.Base;
using sar.Tools;

namespace WinServiceLauncher.Commands
{
	public class Install : sar.Base.Command
	{
		public Install(sar.Base.CommandHub parent) : base(parent, "Install",
		                                                  new List<string> { "install", "i" },
		                                                  @"-i",
		                                                  new List<string> { "-i" })
		{
		}
		
		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}
			
			Progress.Message = "Installing Service";
			List<string> installArgs = new List<string>();
			
			
			if (this.commandHub.NoWarning) installArgs.Add(@"/LogToConsole = false");
			if (!this.commandHub.NoWarning) installArgs.Add(@"/LogToConsole = true");

			installArgs.Add(Assembly.GetExecutingAssembly().Location);
			
			ManagedInstallerClass.InstallHelper(installArgs.ToArray());
			
			ConsoleHelper.WriteLine("");
			ConsoleHelper.WriteLine("Install Complete", ConsoleColor.Yellow);

			return ConsoleHelper.EXIT_OK;
		}
	}
}
