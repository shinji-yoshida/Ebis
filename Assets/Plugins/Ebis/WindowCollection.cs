using UnityEngine;
using System.Collections;

namespace Ebis {
	public interface WindowCollection {
		T FindPrefab<T> (string variation) where T : Window;
	}
}