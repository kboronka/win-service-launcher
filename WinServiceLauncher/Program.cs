/* Copyright (C) 2021 Kevin Boronka
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

using sar.Tools;
using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using Base = sar.Base;

namespace WinServiceLauncher
{
	internal sealed class Program : Base.Program
	{
		private static bool exitSystem = false;

		#region Trap application termination

		[DllImport("Kernel32")]
		private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

		private delegate bool EventHandler(CtrlType sig);

		private static EventHandler _handler;

		private enum CtrlType
		{
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT = 1,
			CTRL_CLOSE_EVENT = 2,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT = 6
		}

		private static bool Handler(CtrlType sig)
		{
			ConsoleHelper.WriteLine("Shutting Down");

			WinServiceLauncher.StopServices();
			Thread.Sleep(250);

			//allow main to run off
			exitSystem = true;

			ConsoleHelper.WriteLine("Shut Down Complete");

			//shutdown right away so there are no lingering threads
			Environment.Exit(-1);

			return true;
		}

		#endregion Trap application termination

		private static void Main(string[] args)
		{
			try
			{
				if (!System.Environment.UserInteractive)
				{
					ServiceBase.Run(new ServiceBase[] { new WinServiceLauncher() });
				}
				else
				{
					try
					{
						sar.Logger.LogToConsole = true;
						_handler += new EventHandler(Handler);
						SetConsoleCtrlHandler(_handler, true);

						var hub = new CommandHub();
						ConsoleHelper.ApplicationShortTitle();
						hub.ProcessCommands(args);
					}
					catch (Exception ex)
					{
						ConsoleHelper.WriteException(ex);
					}

					WinServiceLauncher.StopServices();
					return;
				}
			}
			catch
			{
			}
		}

		public static void Log(Exception ex)
		{
			sar.Logger.Log(ex.Message);
		}

		public static void Log(string message)
		{
			sar.Logger.Log(message);
		}
	}
}