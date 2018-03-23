using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebis.SafeArea {
	public class SafeAreaRectTransformResizing : MonoBehaviour, ISafeAreaComponent {
		[SerializeField] bool resizeBySafeAreaTopPadding;
		[SerializeField] bool resizeBySafeAreaBottomPadding;
		[SerializeField] bool resizeBySafeAreaLeftPadding;
		[SerializeField] bool resizeBySafeAreaRightPadding;

		public void Apply (CanvasProperty property) {
			var rectTransform = GetComponent<RectTransform> ();
			var rect = rectTransform.rect;
			if (resizeBySafeAreaTopPadding || resizeBySafeAreaBottomPadding) {
				if (resizeBySafeAreaTopPadding)
					rect.height += property.safeTopPadding;
				if (resizeBySafeAreaBottomPadding)
					rect.height += property.safeBottomPadding;
				rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, rect.height);
			}
			if (resizeBySafeAreaLeftPadding || resizeBySafeAreaRightPadding) {
				if (resizeBySafeAreaLeftPadding)
					rect.width += property.safeLeftPadding;
				if (resizeBySafeAreaRightPadding)
					rect.width += property.safeRightPadding;
				rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, rect.width);
			}
		}
	}
}
