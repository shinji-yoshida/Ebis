using UniPromise;
using UniRx;

namespace Ebis {
	public interface WindowTransition {
		Promise<Unit> Open ();

		Promise<Unit> Close ();
	}
}
