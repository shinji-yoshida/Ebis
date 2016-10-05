using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Ebis {
	public class StackWindowSpace : WindowSpace {
		List<Window> windows;

		public StackWindowSpace (Transform windowContainer) : base(windowContainer)
		{
			windows = new List<Window> ();
		}


		protected override void AddWindow(Window newWindow) {
			if (windows.Count > 0) {
				windows.Last ().Lock ();
			}

			windows.Add (newWindow);
		}

		public override bool IsTopWindow(Window child) {
			return windows.Last () == child;
		}

		protected override void RemoveWindow(Window child) {
			windows.Remove (child);
		}

		protected override void AfterWindowRemoved(bool wasTop) {
			if (windows.Count > 0 && wasTop) {
				windows.Last ().Unlock ();
			}
		}

		public override bool Contains (Window child) {
			return windows.Contains (child);
		}
	}
}