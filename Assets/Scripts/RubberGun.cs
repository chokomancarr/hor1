using UnityEngine;
using System.Collections;

public class RubberGun : Weapon {

    public GameObject rubber;

    public override void Fire()
    {
        StartCoroutine(DoFire());
    }

    public override void Refresh()
    {
        rubber.SetActive(Ppl.instance.bullets[0].curr > 0);
    }

    IEnumerator DoFire()
    {
        Ppl.instance.canChangeWep = false;
        canFire = false;
        rubber.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (Ppl.instance.bullets[0].curr > 0) rubber.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        canFire = true;
        Ppl.instance.canChangeWep = true;
    }
}
