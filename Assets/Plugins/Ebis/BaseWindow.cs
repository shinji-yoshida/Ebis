using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lockables;

namespace Ebis{
	public abstract class BaseWindow : Window {
//		List<ILockable> lockables = new List<ILockable>();
		CompositeLockable lockables = new CompositeLockable();
		
		public void AddLockable(ILockable lockable){
			lockables.Add (lockable);
		}
		
		public void Lock () {
			lockables.Lock ();
		}
		
		public void Unlock () {
			lockables.Unlock ();
		}

		public void ForceUnlock () {
			lockables.ForceUnlock ();
		}

		public bool IsLocked () {
			return lockables.IsLocked ();
		}
	}
}