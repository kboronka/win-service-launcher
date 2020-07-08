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
	public class Start : sar.Base.Command
	{
		public Start(sar.Base.CommandHub parent) : base(parent, "Start",
		                                                new List<string> { "start" },
		                                                @"-start",
		                                                new List<string> { "-start" })
		{
		}
		
		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}
			
			ConsoleHelper.WriteLine("Starting Service");
			bool success = ServiceHelper.TryStart("WinServiceLauncher.exe");
			
			if (success)
			{
				ConsoleHelper.WriteLine("Service Started", ConsoleColor.Yellow);
				return ConsoleHelper.EXIT_OK;
			}
			else
			{
				ConsoleHelper.WriteLine("Service Failed to Start", ConsoleColor.Red);
				return ConsoleHelper.EXIT_ERROR;
			}
		}
	}
}
