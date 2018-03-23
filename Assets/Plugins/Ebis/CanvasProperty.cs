using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebis {
	public class CanvasProperty {
		public readonly float width;
		public readonly float height;
		public readonly int safeTopPadding;
		public readonly int safeBottomPadding;
		public readonly int safeLeftPadding;
		public readonly int safeRightPadding;

		public CanvasProperty (float width, float height)
		{
			this.width = width;
			this.height = height;
			var ratio = this.height / Screen.height;
			var safeArea = SafeArea.SafeAreaAccessor.SafeArea;
			safeBottomPadding = Mathf.RoundToInt(safeArea.yMin * ratio);
			safeTopPadding = Mathf.RoundToInt((Screen.height - safeArea.yMax) * ratio);
			safeLeftPadding = Mathf.RoundToInt (safeArea.xMin * ratio);
			safeRightPadding = Mathf.RoundToInt((Screen.width - safeArea.xMax) * ratio);
		}
	}
}
