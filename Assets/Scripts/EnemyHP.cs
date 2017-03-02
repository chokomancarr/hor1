using UnityEngine;

public class EnemyHP : MonoBehaviour {
    public float hp = 100;
    public GameObject bloodPar;
    public bool die = false;
    public Animator anim;

    void Start () {
        anim = GetComponent<Animator>();
    }

    public void Dmg (float f, Transform t, Vector3 pos, Vector3 nrm) {
        if (!die) {
            hp -= f;
            if (hp < 0)
                die = true;
            if (f > 15) {
                if (bloodPar) {
                    GameObject o = Instantiate(bloodPar, pos, Quaternion.identity) as GameObject;
                    o.transform.forward = nrm;
                    o.transform.parent = t;
                }
                anim.Play("Dmg", 0, 0);
            }
        }
    }
}
