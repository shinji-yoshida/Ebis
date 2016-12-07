using UnityEngine;
using System.Collections;
using Ebis;
using Lockables;
using UniRx;
using UniPromise;

namespace Ebis {
	public enum WindowStatusType {
		Initial, Opening, Opened, Closing, Closed
	}

	public struct WindowEvent {
		public readonly WindowStatusType type;

		public WindowEvent (WindowStatusType type)
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
		Deferred<Unit> deferredOpened;
		WindowStatusType windowStatus;

		protected void Awake() {
			onEventSubject = new Subject<WindowEvent> ();
			transition = ImmediateWindowTransition.Default;
			deferredOpened = new Deferred<Unit> ();
			windowStatus = WindowStatusType.Initial;
		}

		public void ChangeWindowTransition(WindowTransition newTransition) {
			this.transition = newTransition;
		}

		public WindowStatusType WindowStatus {
			get {
				return windowStatus;
			}
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
			OnOpenedAsObservable ().Subscribe (_ => {
				Unlock ();
				deferredOpened.Resolve(Unit.Default);
			}).AddTo (this);
			OnClosingAsObservable ().Subscribe (_ => Lock ()).AddTo (this);
		}

		internal Promise<Unit> Open(WindowSpace parentSpace) {
			this.parentSpace = parentSpace;
			return transition.Open ();
		}

		public virtual void Close() {
			if (WindowStatus == WindowStatusType.Closing || WindowStatus == WindowStatusType.Closed)
				return;
			
			parentSpace.Close (this, transition.Close());
		}

		public void CloseImmediately() {
			parentSpace.Close (this, Promises.Resolved(Unit.Default));
		}

		public void DestroyWindow() {
			Destroy (gameObject);
		}

		public void NotifyOnOpening() {
			windowStatus = WindowStatusType.Opening;
			OnOpening ();
			onEventSubject.OnNext (new WindowEvent(WindowStatusType.Opening));
		}

		protected virtual void OnOpening () {}

		public void NotifyOnOpened() {
			windowStatus = WindowStatusType.Opened;
			OnOpened ();
			onEventSubject.OnNext (new WindowEvent(WindowStatusType.Opened));
		}

		protected virtual void OnOpened () {}

		public void NotifyOnClosing () {
			windowStatus = WindowStatusType.Closing;
			OnClosing ();
			onEventSubject.OnNext(new WindowEvent(WindowStatusType.Closing));
		}

		protected virtual void OnClosing() {}

		public void NotifyOnClosed () {
			windowStatus = WindowStatusType.Closed;
			OnClosed ();
			onEventSubject.OnNext (new WindowEvent (WindowStatusType.Closed));
		}

		protected virtual void OnClosed () {}

		public IObservable<WindowEvent> OnEventAsObservable() {
			return onEventSubject;
		}

		public IObservable<WindowEvent> OnOpeningAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowStatusType.Opening);
		}

		public IObservable<WindowEvent> OnOpenedAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowStatusType.Opened);
		}

		public IObservable<WindowEvent> OnClosingAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowStatusType.Closing);
		}

		public IObservable<WindowEvent> OnClosedAsObservable() {
			return OnEventAsObservable().Where(e => e.type == WindowStatusType.Closed);
		}

		public Promise<Unit> PromiseOpened {
			get {
				return deferredOpened;
			}
		}
	}
}