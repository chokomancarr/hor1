using UnityEngine;

public class TrueItemDrop : ItemDrop
{
	public ItemType id;
    protected override void Do()
    {
		Ppl.instance.inventoryItems.Add (id);
		Destroy(gameObject);
    }
}

public enum ItemType {
	Undefined = -1,
	GateKey
};
