using UnityEngine;
using System.Collections;

namespace Ebis {
	public interface WindowCollection {
		T FindPrefab<T> () where T : Window;
	}
}