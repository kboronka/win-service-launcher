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
				
				// find
				string dotNetFolder = IO.FindDotNetFolder("4.0");
				ConsoleHelper.DebugWriteLine("dotNetFolder = " + dotNetFolder);
				string installUtil = IO.FindFile(dotNetFolder, "Installutil.exe");
				
				// rename service exe file
				string serviceBuildEXE = IO.FindFile("demo_service.exe");
				string root = IO.GetRoot(serviceBuildEXE);
				string serviceBuildFileName = IO.GetFilename(serviceBuildEXE);
				string serviceDemoEXE = root + StringHelper.TrimEnd(serviceBuildFileName, IO.GetFileExtension(serviceBuildEXE).Length + 1) + "_DEBUG.exe";
				if (File.Exists(serviceDemoEXE)) File.Delete(serviceDemoEXE);
				File.Copy(serviceBuildEXE, serviceDemoEXE);
				
				ShellResults results = Shell.Run(installUtil, "/i " + IO.GetFilename(serviceDemoEXE));
				ConsoleHelper.DebugWriteLine(results.Output);
				ConsoleHelper.DebugWriteLine(ConsoleHelper.HR);
				ConsoleHelper.WriteLine(IO.GetFilename(serviceDemoEXE) + " installed");
				ConsoleHelper.ReadKey();
				
				results = Shell.Run(IO.System32 + @"net.exe", "START " + StringHelper.TrimEnd(serviceBuildFileName, IO.GetFileExtension(serviceBuildEXE).Length + 1));
				ConsoleHelper.DebugWriteLine(results.Output);
				ConsoleHelper.DebugWriteLine(ConsoleHelper.HR);
				ConsoleHelper.ReadKey();
				
				results = Shell.Run(@"C:\Windows\System32\NET.exe", "STOP " + StringHelper.TrimEnd(serviceBuildFileName, IO.GetFileExtension(serviceBuildEXE).Length + 1));
				ConsoleHelper.DebugWriteLine(results.Output);
				ConsoleHelper.DebugWriteLine(ConsoleHelper.HR);
				ConsoleHelper.ReadKey();
				
				ConsoleHelper.WriteLine(IO.GetFilename(serviceDemoEXE) + " uninstalling");
				results = Shell.Run(installUtil, "/u " + IO.GetFilename(serviceDemoEXE));
				ConsoleHelper.DebugWriteLine(results.Output);
				ConsoleHelper.DebugWriteLine(ConsoleHelper.HR);
				ConsoleHelper.WriteLine(IO.GetFilename(serviceDemoEXE) + " uninstalled");
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