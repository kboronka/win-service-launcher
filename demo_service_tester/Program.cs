/*
 * Created by SharpDevelop.
 * User: kboronka
 * Date: 2013-05-22
 * Time: 3:36 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using skylib.Tools;

namespace demo_service_tester
{
	class Program
	{
		public static void Main(string[] args)
		{
			ConsoleHelper.WriteTitle();
			
			// TODO: Implement Functionality Here
			ServiceHelper.LogEvent("test");
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}