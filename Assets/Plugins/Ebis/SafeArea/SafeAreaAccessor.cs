using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebis.SafeArea {
	public class SafeAreaAccessor {
		static SafeAreaAccessor soleInstance = new SafeAreaAccessor ();

		public static void Reset(SafeAreaAccessor newInstance) {
			soleInstance = newInstance;
		}

		public static void Reset() {
			soleInstance = new SafeAreaAccessor ();
		}

		public static Rect SafeArea {
			get {
				return soleInstance.GetSafeArea();
			}
		}

		protected virtual Rect GetSafeArea() {
			return Screen.safeArea;
		}
	}
}
