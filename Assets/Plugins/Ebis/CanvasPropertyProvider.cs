using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniPromise;

namespace Ebis {
	public class CanvasPropertyProvider : MonoBehaviour {
		[SerializeField] Canvas canvas;
		[SerializeField] GameObject windowContainer;
		Deferred<CanvasProperty> _deferredProperty;

		public GameObject WindowContainer {
			get {
				return windowContainer;
			}
		}

		Deferred<CanvasProperty> DeferredProperty {
			get {
				return _deferredProperty ?? (_deferredProperty = new Deferred<CanvasProperty>());
			}
		}

		public Promise<CanvasProperty> GetCanvasProperty() {
			return DeferredProperty;
		}

		void Start () {
			var property = new CanvasProperty (canvas.pixelRect.width / canvas.scaleFactor, canvas.pixelRect.height / canvas.scaleFactor);
			DeferredProperty.Resolve (property);
		}
	}
}