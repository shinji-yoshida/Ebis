using System;
using gotanda;
using UnityEngine;

namespace Ebis {
	public abstract class WindowSpace {
		Transform windowContainer;

		public WindowSpace (Transform windowContainer)
		{
			this.windowContainer = windowContainer;
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

			AddWindow (result);

			result.Open (this);

			result.Lock ();

			result.NotifyOnOpening ();
			result.Unlock ();
			result.NotifyOnOpened ();

			return result;
		}

		protected abstract void AddWindow (Window newWindow);



		public void Close(Window child) {
			Assertion._assert_ (Contains (child));

			var wasTop = IsTopWindow (child);

			child.Lock ();
			child.NotifyOnClosing ();
			child.NotifyOnClosed ();

			RemoveWindow (child);

			child.DestroyWindow ();

			AfterWindowRemoved (wasTop);
		}

		public abstract bool IsTopWindow (Window child);

		public abstract bool Contains (Window child);

		protected abstract void RemoveWindow (Window child);

		protected abstract void AfterWindowRemoved(bool wasTop);
	}
}
