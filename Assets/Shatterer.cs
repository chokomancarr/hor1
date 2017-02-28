using UnityEngine;
using System.Collections;

public class Shatterer : Exec {
    public bool selfCol;
    public Transform ori, shatPar, forcePos;
    public Vector3 random, random2;
    public Vector3 force;
    public float forceSize, staticTime = 5;

	void Start () {
        for (int a = shatPar.childCount-1; a >= 0; a--)
        {
            GameObject t = shatPar.GetChild(a).gameObject;
            t.AddComponent<MeshCollider>().convex = true;
            t.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            t.layer = selfCol? 10 : 11;
        }
	}

    public override void Do ()
    {
        StartCoroutine(DoShatter());
    }
	
	public IEnumerator DoShatter () {
        shatPar.parent = ori.parent;
        Destroy(ori.gameObject);
        for (int a = shatPar.childCount - 1; a >= 0; a--)
        {
            GameObject t = shatPar.GetChild(a).gameObject;
            t.SetActive(true);
            t.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(random.x, random2.x), Random.Range(random.y, random2.y), Random.Range(random.z, random2.z)), ForceMode.Impulse);
        }
        yield return new WaitForSeconds(staticTime);
        for (int a = shatPar.childCount - 1; a >= 0; a--)
        {
            GameObject t = shatPar.GetChild(a).gameObject;
            Destroy(t.GetComponent<MeshCollider>());
            Destroy(t.GetComponent<Rigidbody>());
        }
    }
}
