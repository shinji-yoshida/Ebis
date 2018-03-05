using UnityEngine;
using System.Collections;
using Ebis;
using Lockables;
using UniRx;
using UniPromise;
using Ebis.SafeArea;

namespace Ebis {
	public enum WindowStatusType {
		Initial, Opening, Opened, Closing, Closed
	}

	public class WindowEvent {
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
		Deferred<CUnit> deferredOpened;
		WindowStatusType windowStatus;

		protected void Awake() {
			onEventSubject = new Subject<WindowEvent> ();
			transition = ImmediateWindowTransition.Default;
			deferredOpened = new Deferred<CUnit> ();
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

		public CanvasProperty CanvasProperty {
			get {
				return parentSpace.canvasProperty;
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

		public IObservable<bool> OnLockUpdatedAsObservable () {
			return lockables.OnLockUpdatedAsObservable ();
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
				deferredOpened.Resolve(CUnit.Default);
			}).AddTo (this);
			OnClosingAsObservable ().Subscribe (_ => Lock ()).AddTo (this);
		}

		internal Promise<CUnit> Open(WindowSpace parentSpace) {
			this.parentSpace = parentSpace;

			foreach (var each in GetComponentsInChildren<SafeAreaLayoutGroupPadding>())
				each.Initialize (parentSpace.canvasProperty);
			foreach (var each in GetComponentsInChildren<SafeAreaLayoutElementResizing>())
				each.Initialize (parentSpace.canvasProperty);
			
			return transition.Open ().AddTo(this);
		}

		public virtual void Close() {
			if (WindowStatus == WindowStatusType.Closing || WindowStatus == WindowStatusType.Closed)
				return;
			
			parentSpace.Close (this, transition.Close().AddTo(this));
		}

		public void CloseImmediately() {
			if (WindowStatus == WindowStatusType.Closing || WindowStatus == WindowStatusType.Closed)
				return;
			
			CloseWithoutTransition ();
		}

		public void CloseWithoutTransition() {
			parentSpace.Close (this, Promises.Resolved(CUnit.Default));
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

		public Promise<CUnit> PromiseOpened {
			get {
				return deferredOpened;
			}
		}
	}
}