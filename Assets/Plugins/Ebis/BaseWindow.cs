using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ebis{
	public abstract class BaseWindow : Window {
		List<ILockable> lockables = new List<ILockable>();
		
		public void AddLockable(ILockable lockable){
			lockables.Add(lockable);
		}
		
		public void Lock () {
			foreach(var each in lockables)
				each.Lock();
		}
		
		public void Unlock () {
			foreach(var each in lockables)
				each.Unlock();
		}
	}
}