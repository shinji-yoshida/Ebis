using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ebis.SafeArea {
	public class SafeAreaLayoutGroupPadding : MonoBehaviour {
		[SerializeField] LayoutGroup _layoutGroup;
		[SerializeField] bool paddingTop;
		[SerializeField] bool paddingBottom;
		[SerializeField] bool paddingLeft;
		[SerializeField] bool paddingRight;

		public LayoutGroup LayoutGroup {
			get {
				return _layoutGroup ?? (_layoutGroup = GetComponent<LayoutGroup>());
			}
		}

		public void Initialize(CanvasProperty property) {
			if (!PaddingEnabled) {
				return;
			}
			var padding = LayoutGroup.padding;
			if (paddingTop)
				padding.top += property.safeTopPadding;
			if (paddingBottom)
				padding.bottom += property.safeBottomPadding;
			if (paddingLeft)
				padding.left += property.safeLeftPadding;
			if (paddingRight)
				padding.right += property.safeRightPadding;
			
			LayoutGroup.padding = padding;
		}

		bool PaddingEnabled {
			get {
				return paddingTop || paddingBottom || paddingLeft || paddingRight;
			}
		}
	}
}
