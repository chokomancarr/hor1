using UnityEngine;

public class ZombieDeleteTrigger : MonoBehaviour {
    public Zombie zombie;

    void OnTriggerEnter ()
    {
        if (zombie)
            Destroy(zombie.gameObject);
        Destroy(this);
    }
}
