using UnityEngine;
using System.Collections;

namespace Yudiz.Gaming.Engine
{
	public class Notification
	{
	
		public Component Sender;
		public string FunctionName;
		public object Data;

		public Notification (Component sender, string functionName)
		{
			Sender = sender;
			FunctionName = functionName;
			Data = null;
		}

		public Notification (Component sender, string functionName, object data)
		{
			Sender = sender;
			FunctionName = functionName;
			Data = data;
		}
	}
}