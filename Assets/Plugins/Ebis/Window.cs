using UnityEngine;
using System.Collections;
using Ebis;
using Lockables;
using UniRx;

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
	CompositeLockable lockables = new CompositeLockable();
	Subject<WindowEvent> onEventSubject;

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

	public void Close() {
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
