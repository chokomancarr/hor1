using UnityEngine;
using System.Collections;

public class MoveDistantTrigger : MonoBehaviour {
    public Transform t;
    public Transform dir;
    public float mvSp, killDist;

    bool doing;
    Vector3 oriPos;
    
    void Start ()
    {
        oriPos = t.position;
    }

	void Update () {
	    if (doing)
        {
            t.Translate(0, 0, mvSp * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(oriPos, t.position)) > killDist)
            {
                Destroy(t.gameObject);
                Destroy(this);
            }
        }
	}

    void OnTriggerStay()
    {
        if (!doing)
        {
            if (Mathf.Abs(Quaternion.Angle(Ppl.instance.transform.rotation, dir.rotation)) < 40)
            {
                doing = true;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
