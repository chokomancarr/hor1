using UnityEngine;
using System.Collections;

public class DoorCloser : MonoBehaviour {
    public Door door;
    public bool once;
	
    void OnTriggerEnter ()
    {
        door.Close();
        if (once)
            Destroy(this);
    }
}
