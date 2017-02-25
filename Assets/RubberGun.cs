using UnityEngine;
using System.Collections;

public class RubberGun : Weapon {

    public GameObject rubber;

    public override void Fire()
    {
        StartCoroutine(DoFire());
    }

    IEnumerator DoFire()
    {
        Ppl.instance.canChangeWep = false;
        canFire = false;
        rubber.SetActive(false);
        yield return new WaitForSeconds(1.1f);
        rubber.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        canFire = true;
        Ppl.instance.canChangeWep = true;
    }
}
