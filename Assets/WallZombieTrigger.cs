using UnityEngine;
using System.Collections;

public class WallZombieTrigger : MonoBehaviour {
    public Zombie zombie;
    public Collider deleteCol;

    void Start ()
    {
        zombie.anim.Play("waitWall");
    }

    void OnTriggerEnter ()
    {
        zombie.doWall();
        GetComponent<Collider>().enabled = false;
        deleteCol.enabled = true;
        //StartCoroutine(Kill());
        Destroy(this);
    }

    IEnumerator Kill ()
    {
        yield return new WaitForSeconds(1);
        HUD.instance.Talk("What the hell is that.");
        Destroy(this);
    }
}
