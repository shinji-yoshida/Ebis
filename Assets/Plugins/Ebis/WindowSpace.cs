﻿using System;
using UnityEngine;
using UniPromise;
using UniRx;
using Lockables;
using System.Collections.Generic;

namespace Ebis {
	public abstract class WindowSpace : ILockable {
		Transform windowContainer;
		protected CompositeLockable windowSpaceLockables;
		Subject<Window> windowAddedSubject;
		public readonly CanvasProperty canvasProperty;

		public WindowSpace (Transform windowContainer, CanvasProperty canvasProperty) {
			this.windowContainer = windowContainer;
			this.canvasProperty = canvasProperty;
			windowSpaceLockables = new CompositeLockable ();
			windowAddedSubject = new Subject<Window> ();
		}

		public IObservable<Window> OnWindowAddedAsObservable() {
			return windowAddedSubject;
		}

		public T Open<T>(Action<T, CanvasProperty> onInstantiated=null) where T : Window {
			return Open<T> (null, onInstantiated);
		}

		public T Open<T>(Action<T> onInstantiated=null) where T : Window
		{
			Action<T, CanvasProperty> callback = onInstantiated != null ? (t, canvasProperty) => onInstantiated(t) : (Action<T, CanvasProperty>) null; 
			return Open<T> (null, callback);
		}

		public T Open<T>(string variation, Action<T> onInstantiated=null) where T : Window {
			Action<T, CanvasProperty> callback = onInstantiated != null ? (t, canvasProperty) => onInstantiated(t) : (Action<T, CanvasProperty>) null;
			return Open<T> (variation, callback);
		}

		public T Open<T>(string variation, Action<T, CanvasProperty> onInstantiated=null) where T : Window {
			var prefab = WindowSystem.Instance.FindPrefab<T> (variation);
			Debug.Assert (prefab != null, typeof(T).Name);

			var obj = GameObject.Instantiate(prefab);
			obj.transform.SetParent(windowContainer, false);
			var result = obj.GetComponent<T> ();

			result.OnInstantiated ();

			if(onInstantiated != null)
				onInstantiated (result, canvasProperty);

			AddWindow (result);
			windowAddedSubject.OnNext (result);

			var promiseOpen = result.Open (this);
			result.NotifyOnOpening ();

			promiseOpen.Done (_ => {
				result.NotifyOnOpened ();
			});

			return result;
		}

		protected abstract void AddWindow (Window newWindow);



		public void Close(Window child, Promise<CUnit> closeTransition) {
			Debug.Assert (Contains (child), child.ToString());

			child.NotifyOnClosing ();

			closeTransition.Done (_ => {
				var wasTop = IsTopWindow (child);
				RemoveWindow (child);
				AfterWindowRemoved (wasTop);

				child.NotifyOnClosed ();
				child.DestroyWindow ();

			});
		}

		public abstract bool IsTopWindow (Window child);

		/// returns all windows in WindowSpace, sorted by top to bottom (foreground to background).
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
