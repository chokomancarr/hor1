using UnityEngine;

public class Manager : MonoBehaviour {
    public static Manager instance;

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
