using UnityEngine;

public class DieExec : MonoBehaviour {
    public Exec s;

	void OnDestroy () {
        if (s)
            s.Do();
	}
}
