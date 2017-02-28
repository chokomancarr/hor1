using UnityEngine;

public class DmgPart : MonoBehaviour {
    EnemyHP scr;
    [Range(0, 1)]
    public float eff;

	void Start () {
        Transform t = transform;
        do {
            scr = t.GetComponent<EnemyHP>();
            t = t.parent;
        } while (!scr && t);
        if (!scr)
            Debug.LogError("Cannot find EnemyHP for " + transform.name);
	}

    public void Hit(float dmg, RaycastHit info) {
        scr.Dmg(dmg*eff, transform, info.point, info.normal);
    }
}
