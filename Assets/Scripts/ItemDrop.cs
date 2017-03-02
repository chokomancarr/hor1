using UnityEngine;

public class ItemDrop : MonoBehaviour {
    public string desc;
    float dh;
    bool canInter, inView;
    GUIStyle style;

    void Start ()
    {
        dh = Screen.height * 0.05f;
        style = new GUIStyle();
        style.fontSize = (int)dh;
        style.normal.textColor = Color.white;
    }

    void OnGUI ()
    {
        if (Event.current.type == EventType.repaint && canInter)
        {
            Vector3 p = Ppl.instance.cam.WorldToViewportPoint(transform.position);
            inView = p.z > 0 && p.x > 0 && p.y > 0 && p.x < 1 && p.y < 1;
            if (inView) {
                HUD.instance.SetF(p, desc);
            }
        }
    }

    void OnTriggerEnter()
    {
        canInter = true;
    }

    void Update ()
    {
        if (Input.GetKeyDown(InKeys.Key("F")) && canInter)
        {
            Do();
        }
    }

    protected virtual void Do () { }

    void OnTriggerExit ()
    {
        canInter = false;
    }
}
