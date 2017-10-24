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
			windowSpaceLockables.Add (newWindow);
		}

		public override bool IsTopWindow(Window child) {
			return windows.Last () == child;
		}

		public Window TopWindow {
			get {
				if (windows.Count > 0)
					return windows.Last ();
				else
					return null;
			}
		}

		protected override void RemoveWindow(Window child) {
			windows.Remove (child);
			windowSpaceLockables.Remove (child);
		}

		protected override void AfterWindowRemoved(bool wasTop) {
			if (windows.Count > 0 && wasTop) {
				windows.Last ().Unlock ();
			}
		}

		public override bool Contains (Window child) {
			return windows.Contains (child);
		}

		public override IEnumerable<Window> AllWindows {
			get {
				return windows;
			}
		}

		public override int WindowCount {
			get {
				return windows.Count;
			}
		}
	}
}