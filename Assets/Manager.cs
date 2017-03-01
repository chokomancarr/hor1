using UnityEngine;
using System.Collections.Generic;

public class Manager : MonoBehaviour {
    public static Manager instance;

    public static List<InTypeChangeMat> inTypeMatScrs = new List<InTypeChangeMat>();
    public static void InTypeMatScrDo() { foreach (InTypeChangeMat m in inTypeMatScrs) m.Set(); }

    public Texture tex_black, tex_white;

    public AudioClip[] effectClips;

	void Awake () {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
}
