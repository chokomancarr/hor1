using UnityEngine;

public class EnemyHP : MonoBehaviour {
    public float hp = 100;
    public GameObject bloodPar;
    public bool die = false;

    public void Dmg (float f, Transform t, Vector3 pos, Vector3 nrm) {
        if (!die) {
            hp -= f;
            if (hp < 0)
                die = true;
            if (bloodPar) {
                GameObject o = Instantiate(bloodPar, pos, Quaternion.identity) as GameObject;
                o.transform.forward = nrm;
                o.transform.parent = t;
            }
        }
    }
}
