using UnityEngine;

public class Weapon : MonoBehaviour {
    public bool canFire;
    public float sz, dmg, delay, dst;
    public Sprite aimDot;
    public virtual void Fire() { }
}
