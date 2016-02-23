using UnityEngine;
using System;

namespace Ebis {
	public class WindowDisposable : IDisposable {
		Window window;

		public WindowDisposable (Window window) {
			this.window = window;
		}

		public void Dispose () {
			if (window == null)
				return;
			var cache = window;
			window = null;
			cache.Close ();
		}
	}
}
