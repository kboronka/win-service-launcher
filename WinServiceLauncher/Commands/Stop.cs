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

using sar.Tools;
using System;
using System.Collections.Generic;

namespace WinServiceLauncher.Commands
{
	public class Stop : sar.Base.Command
	{
		public Stop(sar.Base.CommandHub parent) : base(parent, "Stop",
													  new List<string> { "stop" },
													  @"-stop",
													  new List<string> { "-stop" })
		{
		}

		public override int Execute(string[] args)
		{
			// sanity check
			if (args.Length != 1)
			{
				throw new ArgumentException("incorrect number of arguments");
			}

			ConsoleHelper.WriteLine("Stopping Service");
			bool success = ServiceHelper.TryStop("WinServiceLauncher.exe");

			if (success)
			{
				ConsoleHelper.WriteLine("");
				ConsoleHelper.WriteLine("Service Stopped", ConsoleColor.Yellow);
				return ConsoleHelper.EXIT_OK;
			}
			else
			{
				ConsoleHelper.WriteLine("");
				ConsoleHelper.WriteLine("Service Failed to Stop", ConsoleColor.Red);
				return ConsoleHelper.EXIT_ERROR;
			}
		}
	}
}