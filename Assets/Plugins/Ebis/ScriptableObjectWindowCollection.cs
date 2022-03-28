using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Ebis {
	[CreateAssetMenu]
	public class ScriptableObjectWindowCollection : ScriptableObject, WindowCollection {
		[SerializeField] List<Window> windows;

		public T FindPrefab<T> (string variation) where T : Window {
			if(variation == null)
				return windows.Select (w => w.GetComponent<T> ()).FirstOrDefault (w => w != null);
			else
				return windows.Select (w => w.GetComponent<T> ())
					.Where (w => w != null)
					.FirstOrDefault (w => w.gameObject.name.Equals (variation + typeof(T).Name));
		}
	}
}