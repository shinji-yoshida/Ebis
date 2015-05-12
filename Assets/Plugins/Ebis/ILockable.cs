using UnityEngine;
using System.Collections;


namespace Ebis{
	public interface ILockable {
		void Lock ();
		void Unlock ();
	}
}
