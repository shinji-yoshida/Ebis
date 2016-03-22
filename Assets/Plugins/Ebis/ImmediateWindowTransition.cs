using UnityEngine;
using System.Collections;
using UniPromise;
using UniRx;
using UniPromise.UniRxBridge;

namespace Ebis {
	public class ImmediateWindowTransition : WindowTransition {
		public static readonly ImmediateWindowTransition Default = new ImmediateWindowTransition ();

		public Promise<Unit> Open () {
			return UnitPromise.Resolved;
		}

		public Promise<Unit> Close () {
			return UnitPromise.Resolved;
		}
	}
}
