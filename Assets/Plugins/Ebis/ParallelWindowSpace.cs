﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ebis {
	public class ParallelWindowSpace : WindowSpace {
		List<Window> windows;

		public ParallelWindowSpace (Transform windowContainer) : base(windowContainer)
		{
			windows = new List<Window> ();
		}

		protected override void AddWindow (Window newWindow) {
			windows.Add (newWindow);
		}

		public override bool IsTopWindow (Window child) {
			return child == windows.Last ();
		}

		public override bool Contains (Window child) {
			return windows.Contains (child);
		}

		protected override void RemoveWindow (Window child) {
			windows.Remove (child);
		}

		protected override void AfterWindowRemoved (bool wasTop) {
		}

		public override int WindowCount {
			get {
				return windows.Count;
			}
		}
	}
}
