using UnityEngine;

public class WepDrop : ItemDrop
{
    public int type;
    public string msg;
    public float msgDelay;

    protected override void Do()
    {
        for (int q = 0; q < 4; q++)
        {
            if (Ppl.instance.inventoryWeps[q] == -1)
            {
                Ppl.instance.inventoryWeps[q] = type;
                Ppl.instance.UseWep(q, true);
                Destroy(gameObject);
                if (msg != "")
                    HUD.instance.Talk(msg, msgDelay);
                return;
            }
        }
        print("No free slot!");
    }
}
