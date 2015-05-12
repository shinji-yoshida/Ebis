using UnityEngine;
using System.Collections;
using System;


namespace Ebis{
	public class SimpleLockable : ILockable {
		Action locking;
		Action unlocking;
		
		public SimpleLockable (Action locking, Action unlocking) {
			this.locking = locking;
			this.unlocking = unlocking;
		}
		
		public void Lock () {
			locking();
		}
		
		public void Unlock () {
			unlocking();
		}
	}
}