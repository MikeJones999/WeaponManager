using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Debug logger for writing to log file and echo-ing to console log
 * REF: http://www.thegameengineer.com/blog/2014/02/09/unity-11-logging/
 */

namespace Assets.Scripts.Debugging
{
	public class Debugger : MonoBehaviour
	{

		public string LogFile = "log.txt";
		public bool EchoToConsole = true;
		public bool AddTimeStamp = true;

		private StreamWriter OutputStream;

		static Debugger Singleton = null;


		private void Awake()
		{
			if (Singleton != null)
			{
				UnityEngine.Debug.LogError("Multiple Debugger Singletons exits");
				return;
			}

			Singleton = this;

#if !Final
			//Open the log file to append the new log to it
			OutputStream = new StreamWriter(LogFile, true);
#endif

		}

		public static Debugger Instance
		{
			get { return Singleton; }
		}

		private void OnDestroy()
		{
	#if !Final
			if (OutputStream != null)
			{
				OutputStream.Close();
				OutputStream = null;
			}
	#endif
		}




		//main function to log time and write the message
	private void Write(string message)
		{
#if !Final
			if(AddTimeStamp)
			{
				DateTime now = DateTime.Now;
				message = string.Format("[{0:H:mm:ss.fff}] {1}", now, message);
			}

			if(OutputStream != null)
			{
				OutputStream.WriteLine(message);
				OutputStream.Flush();
			}

			if(EchoToConsole)
			{
				UnityEngine.Debug.Log(message);
			}
#endif
		}



		[Conditional("DEBUG"), Conditional("PROFILE")]
		//Outside method to complete the logging
		public static void Trace(string Message)
		{
#if !Final
			if(Debugger.Instance != null)
			{
				Debugger.Instance.Write(Message);
			}
			else
			{
				//Fallback - if the debugger system has not been implemented yet
				UnityEngine.Debug.Log(Message);
			}
#endif
		}


	}
	
}
