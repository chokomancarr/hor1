using UnityEngine;

public class ItemDrop : MonoBehaviour {
    public int type;

    public string msg;
    public float msgDelay;

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
                HUD.instance.SetF(p);
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
            for (int q = 0; q < 4; q++)
            {
                if (Ppl.instance.inventoryWeps[q] == -1)
                {
                    Ppl.instance.inventoryWeps[q] = type;
                    Ppl.instance.UseWep(q, true);
                    Destroy(gameObject);
                    if (msg != "")
                        HUD.instance.Talk(msg, msgDelay);
                    return;
                }
            }
            print("No free slot!");
        }
    }

    void OnTriggerExit ()
    {
        canInter = false;
    }
}
