using UnityEngine;

public class AmmoDrop : ItemDrop
{
    public int id, amt;
    protected override void Do()
    {
        Ppl.instance.bullets[id].all += amt;
        if (Ppl.instance.bullets[id].curr <= 0)
            Ppl.instance.Reload(id);
        if (Ppl.instance.wepScr)
            Ppl.instance.wepScr.Refresh();
        Destroy(gameObject);
    }
}
