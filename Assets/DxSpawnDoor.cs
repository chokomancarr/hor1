using UnityEngine;

public class DxSpawnDoor : OnE {
    public GameObject dx;
	public override void Do5 ()
    {
        GameObject o = Instantiate(dx, Ppl.instance.transform.position, Ppl.instance.transform.rotation) as GameObject;
        o.transform.Rotate(0, 180, 0);
        o.GetComponent<Animator>().Play("Enter");
    }

    public override void DoEnd()
    {
        GetComponent<Door>().Reset();
        Destroy(this);
    }
}
