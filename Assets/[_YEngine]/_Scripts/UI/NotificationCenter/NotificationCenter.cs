using UnityEngine;
using System;
using System.Collections;
using Yudiz.Gaming.Engine;

public class NotificationCentre : MonoBehaviour
{
 
	private static NotificationCentre instance;
	Hashtable notifications = new Hashtable ();

	delegate void OnNotificationDelegate (Notification note);

	void Awake ()
	{
		gameObject.AddComponent (typeof(DontDestroyOnLoad));
	}

	public static NotificationCentre Instance {
		get {
			if (instance == null) {
				instance = new GameObject ("NotificationCentre").AddComponent<NotificationCentre> ();
			}
			return instance;
		}
	}

	public void OnApplicationQuit ()
	{
		instance = null;
	}

	public static void AddObserver (Component observer, String name)
	{
		if (string.IsNullOrEmpty (name)) {
			Debug.Log ("Null name specificed for notification in AddObserver.");
			return;
		}
		if (Instance.notifications.Contains (name) == false) {
			Instance.notifications [name] = new ArrayList ();
		}
       
		ArrayList notifyList = (ArrayList)Instance.notifications [name];
       
		if (!notifyList.Contains (observer)) {
			notifyList.Add (observer);
		}
       
	}

	public static void RemoveObserver (Component observer, String name)
	{
		ArrayList notifyList = (ArrayList)Instance.notifications [name];
       
		if (notifyList != null) {
			if (notifyList.Contains (observer)) {
				notifyList.Remove (observer);
			}
			if (notifyList.Count == 0) {
				Instance.notifications.Remove (name);
			}
		}
	}

	public static void PostNotification (Component aSender, String aName)
	{
		PostNotification (aSender, aName, null);
	}

	public static void PostNotification (Component aSender, String aName, Hashtable aData)
	{
		PostNotification (new Notification (aSender, aName, aData));
	}

	public static void PostNotification (Notification aNotification)
	{
		if (string.IsNullOrEmpty (aNotification.FunctionName)) {
			Debug.Log ("Null name sent to PostNotification.");
			return;
		}
       
		ArrayList notifyList = (ArrayList)Instance.notifications [aNotification.FunctionName];
       
		if (notifyList == null) {
			Debug.Log ("Notificaiton: " + aNotification.FunctionName +
			" Sent by: " + aNotification.Sender.name + " not found in Notify list.");
			return;
		}
       
		ArrayList observersToRemove = new ArrayList ();
		foreach (Component observer in notifyList) {
			if (!observer) {
				observersToRemove.Add (observer);
			} else {
				observer.SendMessage (aNotification.FunctionName, aNotification.Data, SendMessageOptions.DontRequireReceiver);
			}
		}
       
		foreach (object observer in observersToRemove) {
			notifyList.Remove (observer);
		}
	}
}
