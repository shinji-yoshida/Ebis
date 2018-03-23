using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ebis.SafeArea {
	public class SafeAreaLayoutElementResizing : MonoBehaviour, ISafeAreaComponent {
		[SerializeField] bool resizeBySafeAreaTopPadding;
		[SerializeField] bool resizeBySafeAreaBottomPadding;
		[SerializeField] bool resizeBySafeAreaLeftPadding;
		[SerializeField] bool resizeBySafeAreaRightPadding;
		[SerializeField] bool modifyPreferedSize;
		[SerializeField] LayoutElement _layoutElement;

		public LayoutElement LayoutElement {
			get {
				return _layoutElement ?? (_layoutElement = GetComponent<LayoutElement>());
			}
		}

		public void Apply (CanvasProperty property) {
			if (resizeBySafeAreaTopPadding)
				AddHeight (property.safeTopPadding);
			if (resizeBySafeAreaBottomPadding)
				AddHeight (property.safeBottomPadding);
			if (resizeBySafeAreaLeftPadding)
				AddWidth (property.safeLeftPadding);
			if (resizeBySafeAreaRightPadding)
				AddWidth (property.safeRightPadding);
		}

		void AddHeight (int delta) {
			if (modifyPreferedSize)
				LayoutElement.preferredHeight += delta;
			else
				LayoutElement.minHeight += delta;
		}

		void AddWidth (int delta) {
			if (modifyPreferedSize)
				LayoutElement.preferredWidth += delta;
			else
				LayoutElement.minWidth += delta;
		}
	}
}
