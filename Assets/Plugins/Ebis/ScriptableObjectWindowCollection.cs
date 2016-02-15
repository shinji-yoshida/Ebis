using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Ebis {
	public class ScriptableObjectWindowCollection : ScriptableObject, WindowCollection {
		[SerializeField] List<Window> windows;

		public T FindPrefab<T> () where T : Window {
			return windows.Select (w => w.GetComponent<T> ()).FirstOrDefault (w => w != null);
		}
	}
}