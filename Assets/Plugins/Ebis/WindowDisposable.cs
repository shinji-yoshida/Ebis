using UnityEngine;
using System;

namespace Ebis {
	public class WindowDisposable : IDisposable {
		Window window;
		bool immediate;

		public WindowDisposable (Window window, bool immediate = false) {
			this.window = window;
			this.immediate = immediate;
		}

		public void Dispose () {
			if (window == null)
				return;
			var cache = window;
			window = null;
			if (immediate)
				cache.CloseImmediately ();
			else
				cache.Close ();
		}
	}
}
