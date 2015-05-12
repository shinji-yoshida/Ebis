using UnityEngine;
using System.Collections;
using System;


namespace Ebis{
	public class Lockable {
		public static ILockable Create(Action locking, Action unlocking) {
			return new SimpleLockable(locking, unlocking);
		}
	}
}
