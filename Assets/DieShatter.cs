using UnityEngine;

public class DieShatter : MonoBehaviour {
    public Shatterer s;

	void OnDestroy () {
        if (s)
            s.Shatter();
	}
}
