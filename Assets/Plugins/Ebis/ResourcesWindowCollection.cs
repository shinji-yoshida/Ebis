using UnityEngine;
using System.Collections;
using System.IO;

namespace Ebis {
	public class ResourcesWindowCollection : WindowCollection {
		string basePath;

		public ResourcesWindowCollection (string basePath) {
			this.basePath = basePath;
		}
		

		public T FindPrefab<T> (string variation) where T : Window {
			if(variation == null)
				return Resources.Load<T> (Path.Combine (basePath, typeof(T).Name));
			else
				return Resources.Load<T> (Path.Combine(Path.Combine (basePath, typeof(T).Name), variation + typeof(T).Name));
		}
	}
}
