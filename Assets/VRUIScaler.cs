using UnityEngine;

public class VRUIScaler : MonoBehaviour {
    public float s = 0.5f;
	void Start () {
        if (!Ppl.instance.vr)
            GetComponent<RectTransform>().localScale = Vector3.one * s;
        Destroy(this);
	}
}
