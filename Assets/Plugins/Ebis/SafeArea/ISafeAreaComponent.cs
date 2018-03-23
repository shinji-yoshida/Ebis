using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebis.SafeArea {
	public interface ISafeAreaComponent {
		void Apply (CanvasProperty property);
	}
}
