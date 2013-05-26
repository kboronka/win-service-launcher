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

using skylib.Tools;

namespace demo_service_tester
{
	class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				ConsoleHelper.Start();
				ConsoleHelper.ApplicationTitle();
				
				#if DEBUG
				ShellResults results;
				
				// rename service exe file
				string serviceBuildEXE = IO.FindFile("demo_service.exe");
				string root = IO.GetRoot(serviceBuildEXE);
				string serviceBuildFileName = IO.GetFilename(serviceBuildEXE);
				string serviceName = StringHelper.TrimEnd(serviceBuildFileName, IO.GetFileExtension(serviceBuildEXE).Length + 1);
				
				string serviceDemoEXE = serviceBuildEXE;
				//string serviceDemoEXE = root + serviceName + "_DEBUG.exe";
				//if (File.Exists(serviceDemoEXE)) File.Delete(serviceDemoEXE);
				//File.Copy(serviceBuildEXE, serviceDemoEXE);
				
				
				ServiceHelper.TryStop(serviceDemoEXE);
				ServiceHelper.TryUninstall("4.0", serviceDemoEXE);
				ServiceHelper.Install("4.0", serviceDemoEXE);
				ConsoleHelper.WriteLine(serviceName + " installed");
				
				
				ServiceHelper.Start(serviceDemoEXE);
				ConsoleHelper.WriteLine(serviceName + " started");
				ConsoleHelper.ReadKey();
				
				
				ServiceHelper.Stop(serviceDemoEXE);
				ConsoleHelper.WriteLine(serviceName + " stopped");
				
				ServiceHelper.Uninstall("4.0", serviceDemoEXE);
				ConsoleHelper.WriteLine(serviceName + " uninstalled");
				ConsoleHelper.ReadKey();
				#endif
			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteException(ex);
				ConsoleHelper.ReadKey();
			}
			

			ConsoleHelper.Shutdown();
		}
	}
}