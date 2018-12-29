using UnityEngine;
using Yudiz.Gaming.Engine;

public class DontDestroyOnLoad : _DontDestroyOnLoad
{
}
namespace Yudiz.Gaming.Engine
{
	public abstract class _DontDestroyOnLoad : MonoBehaviour
	{
		protected virtual void Awake ()
		{
			// Set the GameObject name to the class name for easy access from Obj-C
			if (this.gameObject) {
				DontDestroyOnLoad (this);
			}
		}
	}
}