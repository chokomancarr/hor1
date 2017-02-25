using UnityEngine;

public class ItemDrop : MonoBehaviour {
    public int type;

    public string msg;
    public float msgDelay;

    float dh;
    bool canInter;
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
            Vector3 p = Ppl.instance.cam.WorldToScreenPoint(transform.position);
            if (p.z > 0)
            {
                GUI.DrawTexture(new Rect(p.x - dh * 0.5f, Screen.height - p.y - dh * 0.5f, dh * 0.7f, dh), Manager.instance.tex_black);
                GUI.Label(new Rect(p.x - dh * 0.5f, Screen.height - p.y - dh * 0.5f, dh, dh), InKeys.Nm("F"), style);
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
