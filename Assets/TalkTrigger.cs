using UnityEngine;

public class TalkTrigger : MonoBehaviour {
    public string msg;
    public bool once;

    void OnTriggerEnter()
    {
        HUD.instance.Talk(msg);
        if (once)
            Destroy(gameObject);
    }
}
