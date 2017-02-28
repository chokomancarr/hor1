using UnityEngine;

public class BarricadeZombieBreak : Exec
{
    public GameObject okB, bkB;
    public Zombie z;

    public override void Do()
    {
        if (z)
        {
            bkB.SetActive(true);
            Destroy(z.gameObject);
            okB.SetActive(false);
            Destroy(this);
        }
    }
}
