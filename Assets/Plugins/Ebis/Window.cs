using UnityEngine;
using System.Collections;
using Ebis;
using Lockables;
using UniRx;
using UniPromise;

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
		WindowSpace parentSpace;
		WindowTransition transition;

		protected void Awake() {
			onEventSubject = new Subject<WindowEvent> ();
			transition = ImmediateWindowTransition.Default;
		}

		public void ChangeWindowTransition(WindowTransition newTransition) {
			this.transition = newTransition;
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

		internal Promise<Unit> Open(WindowSpace parentSpace) {
			this.parentSpace = parentSpace;
			return transition.Open ();
		}

		public virtual void Close() {
			parentSpace.Close (this, transition.Close());
		}

		public void DestroyWindow() {
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

		public void NotifyOnClosing () {
			OnClosing ();
			onEventSubject.OnNext(new WindowEvent(WindowEventType.Closing));
		}

		protected virtual void OnClosing() {}

		public void NotifyOnClosed () {
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