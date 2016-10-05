using UniPromise;
using UniRx;
using UniPromise.UniRxBridge;
using System;


namespace Ebis {
	public abstract class ModalWindow<TSelection> : Window {
		Promise<Unit> promiseClosed;
		TSelection selection;


		public void InitializeModal() {
			promiseClosed = OnClosedAsObservable ().AsUnitObservable().PromiseOnNext ();
		}

		protected override void OnOpening () {
			UnityEngine.Debug.Assert(promiseClosed != null, "should call InitializeModal() before Open");
			base.OnOpening ();
		}

		public void CloseModal(TSelection selection) {
			this.selection = selection;
			base.Close ();
		}

		public override void Close () {
			throw new Exception ("call CloseModal() instead.");
		}

		public Promise<TSelection> OnModalClosed() {
			return promiseClosed.Select(_ => selection);
		}
	}
}