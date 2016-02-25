using UnityEngine;
using System.Collections.Generic;
using gotanda;
using System;

namespace Ebis {
	public interface WindowSpace {
		void Close (Window child);
	}

	public class StackWindowSpace : WindowSpace {
		List<Window> windows;
		Transform windowContainer;

		public StackWindowSpace (Transform windowContainer)
		{
			this.windowContainer = windowContainer;
			windows = new List<Window> ();
		}
		

		public T Open<T>(Action<T> onInstantiated=null) where T : Window {
			var prefab = WindowSystem.Instance.FindPrefab<T> ();
			Assertion._assert_ (prefab != null);

			var obj = GameObject.Instantiate(prefab);
			obj.transform.SetParent(windowContainer, false);
			var result = obj.GetComponent<T> ();

			result.OnInstantiated ();

			if(onInstantiated != null)
				onInstantiated (result);

			if (windows.Count > 0) {
				windows.Last ().Lock ();
			}

			windows.Add (result);

			result.Open (this);

			result.Lock ();

			result.NotifyOnOpening ();
			result.Unlock ();
			result.NotifyOnOpened ();

			return result;
		}

		public void Close(Window child) {
			Assertion._assert_ (windows.Contains (child));

			var toUnlockHead = windows.Last () == child;

			child.Lock ();
			child.NotifyOnClosing ();
			child.NotifyOnClosed ();

			windows.Remove (child);
			child.DestroyWindow ();

			if (windows.Count > 0 && toUnlockHead) {
				windows.Last ().Unlock ();
			}
		}
	}
}