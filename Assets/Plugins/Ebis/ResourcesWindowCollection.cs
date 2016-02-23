using UnityEngine;
using System.Collections;
using System.IO;

namespace Ebis {
	public class ResourcesWindowCollection : WindowCollection {
		string basePath;

		public ResourcesWindowCollection (string basePath) {
			this.basePath = basePath;
		}
		

		public T FindPrefab<T> () where T : Window {
			return Resources.Load<T> (Path.Combine (basePath, typeof(T).Name));
		}
	}
}
