using UnityEngine;
using System.Collections;

public class SwissKnife : Weapon
{
    public override void Fire()
    {
        StartCoroutine(DoFire());
    }

    IEnumerator DoFire()
    {
        Ppl.instance.canChangeWep = false;
        canFire = false;
        yield return new WaitForSeconds(0.6f);
        canFire = true;
        Ppl.instance.canChangeWep = true;
    }
}
