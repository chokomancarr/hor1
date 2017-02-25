using UnityEngine;

public class ZombieDeleteTrigger : MonoBehaviour {
    public Zombie zombie;

    void OnTriggerEnter ()
    {
        Destroy(zombie.gameObject);
        Destroy(this);
    }
}
