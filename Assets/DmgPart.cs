using UnityEngine;

public class DmgPart : MonoBehaviour {
    EnemyHP scr;
    [Range(0, 1)]
    public float eff;
    public AudioClip clip;
    public AudioSource src;

	protected virtual void Start () {
        Transform t = transform;
        do {
            scr = t.GetComponent<EnemyHP>();
            t = t.parent;
        } while (!scr && t);
        if (!scr)
            Debug.LogError("Cannot find EnemyHP for " + transform.name);
        src = scr.GetComponent<AudioSource>();
	}

    public virtual void Hit(float dmg, RaycastHit info) {
        if (!scr.die) {
            scr.Dmg(dmg * eff, transform, info.point, info.normal);
            if (clip) {
                src.clip = clip;
                src.Play();
            }
        }
    }
}
