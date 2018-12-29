using UnityEngine;
using System.Collections;

namespace Yudiz.Gaming.Engine
{
	[System.Serializable]
	public class MenuEventArgs
	{
		public string Sender = string.Empty;
		public string Value = string.Empty;
		public MenuExecutation ActionType;

		public MenuEventArgs ()
		{
			Value = string.Empty;
			ActionType = MenuExecutation.SelectionTap;
		}

		public MenuEventArgs (string value, MenuExecutation type)
		{
			Sender = string.Empty;
			Value = value;
			ActionType = type;
		}

		public MenuEventArgs (string sender, string value, MenuExecutation type)
		{
			Sender = sender;
			Value = value;
			ActionType = type;
		}
	}
}