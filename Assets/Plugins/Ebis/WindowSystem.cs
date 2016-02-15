﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gotanda;
using System;
using System.Linq;

namespace Ebis{
	public class WindowSystem {
		static WindowSystem soleInstance = new WindowSystem();

		public static WindowSystem Instance {
			get { return soleInstance; }
		}

		List<WindowCollection> windowCollections = new List<WindowCollection>();

		public void AddWindowCollection(WindowCollection collection) {
			windowCollections.Add (collection);
		}

		public T FindPrefab<T>() where T : Window {
			return windowCollections.Select (wc => wc.FindPrefab<T> ()).FirstOrDefault ();
		}
	}
}

