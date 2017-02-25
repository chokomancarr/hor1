using UnityEngine;

public class TalkTrigger1 : MonoBehaviour {
    public string msg;
    public bool once;
    public float angle;

    bool t = false;
    void OnTriggerStay()
    {
        if (!t && (Mathf.DeltaAngle(Ppl.instance.camDirX, transform.eulerAngles.y) < 20) && (Mathf.Abs(Ppl.instance.camDirY - angle) < 20))
        {
            HUD.instance.Talk(msg);
            t = true;
            if (once)
                Destroy(gameObject);
        }
    }

    void OnTriggerExit ()
    {
        t = false;
    }
}
