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
using System.IO;

using sar.Tools;

namespace WinServiceLauncherInstaller
{
	class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				ConsoleHelper.Start();
				ConsoleHelper.ApplicationTitle();
				
				string serviceEXE = IO.FindFile("WinServiceLauncher.exe");
				string serviceFilename = IO.GetFilename(serviceEXE);
				string serviceName = StringHelper.TrimEnd(serviceFilename, IO.GetFileExtension(serviceEXE).Length + 1);
				string serviceRoot = IO.GetRoot(serviceEXE);
				
				ServiceHelper.TryStop(serviceEXE);
				ServiceHelper.TryUninstall(serviceEXE);
				
				
				ServiceHelper.Install("4.0", serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " installed");
				
				
				ServiceHelper.Start(serviceEXE);
				ConsoleHelper.WriteLine(serviceName + " started");
				ConsoleHelper.Write(serviceName + " should be installed and running", ConsoleColor.Yellow);

			}
			catch (Exception ex)
			{
				ServiceHelper.TryStop("WinServiceLauncher");
				ServiceHelper.TryUninstall("WinServiceLauncher");
				ConsoleHelper.WriteException(ex);
				ConsoleHelper.ReadKey();
			}

			ConsoleHelper.Shutdown();
		}
	}
}