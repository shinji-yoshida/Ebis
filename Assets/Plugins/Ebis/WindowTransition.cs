using UniPromise;
using UniRx;

namespace Ebis {
	public interface WindowTransition {
		Promise<CUnit> Open ();

		Promise<CUnit> Close ();
	}
}
