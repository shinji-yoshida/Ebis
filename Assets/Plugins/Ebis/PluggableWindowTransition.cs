using UnityEngine;
using System.Collections;
using UniPromise;
using UniRx;
using System;

namespace Ebis {
	public class PluggableWindowTransition : WindowTransition {
		Func<Promise<Unit>> open;
		Func<Promise<Unit>> close;

		public PluggableWindowTransition (Func<Promise<Unit>> open, Func<Promise<Unit>> close) {
			this.open = open;
			this.close = close;
		}
		
		public Promise<Unit> Open () {
			return open ();
		}

		public Promise<Unit> Close () {
			return close ();
		}
	}
}
