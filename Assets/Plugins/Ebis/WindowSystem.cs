using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gotanda;
using System;

namespace Ebis{
	public class WindowSystem {
		static WindowSystem soleInstance = new WindowSystem();

		public static WindowSystem Instance {
			get { return soleInstance; }
		}

		Stack<Window> windowStack = new Stack<Window>();

		public void DoModal(Window window) {
			if(windowStack.Count > 0)
				windowStack.Peek().Lock();
			windowStack.Push(window);
		}

		public void CloseModal(Window window) {
			var popped = windowStack.Pop();
			Assertion._assert_(popped == window);
			if(windowStack.Count > 0)
				windowStack.Peek().Unlock();
		}
	}
}

