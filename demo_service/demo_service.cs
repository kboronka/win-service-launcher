/*
 * Created by SharpDevelop.
 * User: kboronka
 * Date: 2013-05-22
 * Time: 10:30 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using skylib.Tools;

namespace demo_service
{
	public class demo_service : ServiceBase
	{
		public const string MyServiceName = "demo_service";
		
		public demo_service()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
		}
		
		protected override void Dispose(bool disposing)
		{
			// TODO: Add cleanup code here (if required)
			base.Dispose(disposing);
		}
		
		protected override void OnStart(string[] args)
		{
			ServiceHelper.LogEvent("OnStart");
			// TODO: Add start code here (if required) to start your service.
		}
		
		protected override void OnStop()
		{
			ServiceHelper.LogEvent("OnStop");
			// TODO: Add tear-down code here (if required) to stop your service.
		}
	}
}
