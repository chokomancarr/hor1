using UnityEngine;

public class InTypeChangeMat : MonoBehaviour {
    public Material m, j;
    public int id;

	void Awake () {
        Manager.inTypeMatScrs.Add(this);
	}

    public void Set ()
    {
        Renderer r = GetComponent<Renderer>();
        Material[] ms = r.sharedMaterials;
        ms[id] = (InKeys.isJoystick ? j : m);
        r.sharedMaterials = ms;
    }
}
