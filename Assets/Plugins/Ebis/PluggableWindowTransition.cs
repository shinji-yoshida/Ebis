using UnityEngine;
using System.Collections;
using UniPromise;
using UniRx;
using System;

namespace Ebis {
	public class PluggableWindowTransition : WindowTransition {
		Func<Promise<CUnit>> open;
		Func<Promise<CUnit>> close;

		public PluggableWindowTransition (Func<Promise<CUnit>> open, Func<Promise<CUnit>> close) {
			this.open = open;
			this.close = close;
		}
		
		public Promise<CUnit> Open () {
			return open ();
		}

		public Promise<CUnit> Close () {
			return close ();
		}
	}
}
