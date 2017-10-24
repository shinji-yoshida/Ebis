using System;
using UnityEngine;
using UniPromise;
using UniRx;
using Lockables;
using System.Collections.Generic;

namespace Ebis {
	public abstract class WindowSpace : ILockable {
		Transform windowContainer;
		protected CompositeLockable windowSpaceLockables;

		public WindowSpace (Transform windowContainer)
		{
			this.windowContainer = windowContainer;
			windowSpaceLockables = new CompositeLockable ();
		}

		public T Open<T>(Action<T> onInstantiated=null) where T : Window {
			var prefab = WindowSystem.Instance.FindPrefab<T> ();
			Debug.Assert (prefab != null, typeof(T).Name);

			var obj = GameObject.Instantiate(prefab);
			obj.transform.SetParent(windowContainer, false);
			var result = obj.GetComponent<T> ();

			result.OnInstantiated ();

			if(onInstantiated != null)
				onInstantiated (result);

			AddWindow (result);

			var promiseOpen = result.Open (this);
			result.NotifyOnOpening ();

			promiseOpen.Done (_ => {
				result.NotifyOnOpened ();
			});

			return result;
		}

		protected abstract void AddWindow (Window newWindow);



		public void Close(Window child, Promise<CUnit> closeTransition) {
			Debug.Assert (Contains (child));

			var wasTop = IsTopWindow (child);

			child.NotifyOnClosing ();

			closeTransition.Done (_ => {
				RemoveWindow (child);
				AfterWindowRemoved (wasTop);

				child.NotifyOnClosed ();
				child.DestroyWindow ();

			});
		}

		public abstract bool IsTopWindow (Window child);

		public abstract IEnumerable<Window> AllWindows {
			get;
		}

		public abstract bool Contains (Window child);

		protected abstract void RemoveWindow (Window child);

		protected abstract void AfterWindowRemoved(bool wasTop);

		public abstract int WindowCount { get; }

		public void Lock () {
			windowSpaceLockables.Lock ();
		}

		public void Unlock () {
			windowSpaceLockables.Unlock ();
		}

		public void ForceUnlock () {
			windowSpaceLockables.ForceUnlock ();
		}

		public bool IsLocked () {
			return windowSpaceLockables.IsLocked ();
		}

		public IObservable<bool> OnLockUpdatedAsObservable () {
			return windowSpaceLockables.OnLockUpdatedAsObservable ();
		}
	}
}
