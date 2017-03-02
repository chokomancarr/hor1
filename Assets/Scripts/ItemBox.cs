using UnityEngine;

public class ItemBox : DmgPart
{
    public GameObject item;
    public Transform cover;

    protected override void Start() {}

    public override void Hit(float dmg, RaycastHit info)
    {
        if (item)
            item.SetActive(true);
        src.Play();
        cover.Rotate(0, 180, 0, Space.World);
        Destroy(gameObject);
    }
}
