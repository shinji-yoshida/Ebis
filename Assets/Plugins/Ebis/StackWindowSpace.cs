using UnityEngine;
using System.Collections.Generic;
using gotanda;
using System;

namespace Ebis {
	public class StackWindowSpace {
		Stack<Window> windows;
		Transform windowContainer;

		public StackWindowSpace (Transform windowContainer)
		{
			this.windowContainer = windowContainer;
		}
		

		public T Open<T>(Action<T> onInstantiated=null) where T : Window {
			var prefab = WindowSystem.Instance.FindPrefab<T> ();
			Assertion._assert_ (prefab != null);

			var obj = GameObject.Instantiate(prefab);
			obj.transform.SetParent(windowContainer, false);
			var result = obj.GetComponent<T> ();

			if(onInstantiated != null)
				onInstantiated (result);

			result.NotifyOnOpening ();
			result.NotifyOnOpened ();

			return result;
		}
	}
}