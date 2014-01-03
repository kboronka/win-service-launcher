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
using System.Threading;
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
				
				Progress.Message = "Stopping Service";
				ServiceHelper.TryStop(serviceEXE);
				
				Progress.Message = "Uninstainlling Service";
				ConsoleHelper.TryRun(serviceEXE, "-u");
				
				Progress.Message = "Installing Service";
				ConsoleHelper.TryRun(serviceEXE, "-i");
				Progress.Message = "Starting Service";
				ServiceHelper.Start(serviceEXE);
				
				ConsoleHelper.WriteLine(serviceName + " started");
				ConsoleHelper.Write(serviceName + " should be installed and running", ConsoleColor.Yellow);
				Thread.Sleep(2000);
			}
			catch (Exception ex)
			{
				ServiceHelper.TryStop("WinServiceLauncher");
				ConsoleHelper.TryRun("WinServiceLauncher.exe", "-u");
				ConsoleHelper.WriteException(ex);
				ConsoleHelper.ReadKey();
			}

			ConsoleHelper.Shutdown();
		}
	}
}