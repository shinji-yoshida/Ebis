using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebis.SafeArea {
	public class StubSafeAreaAccessor : SafeAreaAccessor {
		Rect safeArea;

		public StubSafeAreaAccessor(int topPadding, int bottomPadding, int leftPadding, int rightPadding) {
			safeArea = new Rect (leftPadding, topPadding, Screen.width - leftPadding - rightPadding, Screen.height - topPadding - bottomPadding);
		}

		public void Install() {
			SafeAreaAccessor.Reset (this);
		}

		protected override Rect GetSafeArea () {
			return safeArea;
		}
	}
}
