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
using System.ServiceProcess;
using System.Text;

namespace demo_service
{
	static class Program
	{
		/// <summary>
		/// This method starts the service.
		/// </summary>
		static void Main()
		{
			// To run more than one service you have to add them here
			ServiceBase.Run(new ServiceBase[] { new demo_service() });
		}
	}
}
