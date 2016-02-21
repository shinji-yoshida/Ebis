using UnityEngine;
using System.Collections;
using Ebis;
using Lockables;
using UniRx;

namespace Ebis {
	public enum WindowEventType {
		Opening, Opened, Closing, Closed
	}

	public struct WindowEvent {
		public readonly WindowEventType type;

		public WindowEvent (WindowEventType type)
		{
			this.type = type;
		}
	}

	public class Window : MonoBehaviour, ILockable {
		CompositeLockable lockables;
		Subject<WindowEvent> onEventSubject;
		CanvasGroup canvasGroup;

		protected void Awake() {
			onEventSubject = new Subject<WindowEvent> ();
		}

		public void AddLockable(ILockable lockable){
			lockables.Add (lockable);
		}

		public void Lock () {
			lockables.Lock ();
		}

		public void Unlock () {
			lockables.Unlock ();
		}

		public void ForceUnlock () {
			lockables.ForceUnlock ();
		}

		public bool IsLocked () {
			return lockables.IsLocked ();
		}

		internal void OnInstantiated() {
			this.canvasGroup = GetComponent<CanvasGroup> ();
			if (canvasGroup == null)
				canvasGroup = gameObject.AddComponent<CanvasGroup> ();

			lockables = new CompositeLockable ();
			lockables.Add (Lockable.Create (onLocked: () => canvasGroup.interactable = false, onUnlocked: () => canvasGroup.interactable = true));

			OnOpeningAsObservable ().Subscribe (_ => Lock ()).AddTo (this);
			OnOpenedAsObservable ().Subscribe (_ => Unlock ()).AddTo (this);
			OnClosingAsObservable ().Subscribe (_ => Lock ()).AddTo (this);
		}

		public virtual void Close() {
			NotifyOnClosing ();
			NotifyOnClosed ();

			Destroy (gameObject);
		}

		public void NotifyOnOpening() {
			OnOpening ();
			onEventSubject.OnNext (new WindowEvent(WindowEventType.Opening));
		}

		protected virtual void OnOpening () {}

		public void NotifyOnOpened() {
			OnOpened ();
			onEventSubject.OnNext (new WindowEvent(WindowEventType.Opened));
		}

		protected virtual void OnOpened () {}

		void NotifyOnClosing () {
			OnClosing ();
			onEventSubject.OnNext(new WindowEvent(WindowEventType.Closing));
		}

		protected virtual void OnClosing() {}

		void NotifyOnClosed () {
			OnClosed ();
			onEventSubject.OnNext (new WindowEvent (WindowEventType.Closed));
		}

		protected virtual void OnClosed () {}

		public IObservable<WindowEvent> OnEventAsObservable() {
			return onEventSubject;
		}

		public IObservable<WindowEvent> OnOpeningAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowEventType.Opening);
		}

		public IObservable<WindowEvent> OnOpenedAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowEventType.Opened);
		}

		public IObservable<WindowEvent> OnClosingAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowEventType.Closing);
		}

		public IObservable<WindowEvent> OnClosedAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowEventType.Closed);
		}
	}
}