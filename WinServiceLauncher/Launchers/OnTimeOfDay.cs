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
using System.Threading;

namespace WinServiceLauncher.Launchers
{
	public class OnTimeOfDay : Schedule
	{
		private TimeSpan time;

		public OnTimeOfDay(Launcher parent) : base(parent)
		{
		}

		public OnTimeOfDay(Launcher parent, XML.Reader reader) : base(parent, reader)
		{
			time = reader.GetAttributeTimeSpan("time");
		}

		protected override void ServiceLauncher()
		{
			double countDown = DateTime.Now.TimeOfDay.TotalMilliseconds - this.time.TotalMilliseconds;

			if (countDown > 0 && countDown < 500)
			{
				this.Launch();
				Thread.Sleep(90000);
			}
		}

		internal override void Serialize(XML.Writer writer)
		{
			writer.WriteStartElement("OnTimeOfDay");
			writer.WriteAttributeString("time", this.time);
			writer.WriteEndElement();   // OnStartup
		}
	}
}
