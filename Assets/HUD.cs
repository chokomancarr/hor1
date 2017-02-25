using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {
    public static HUD instance;
    public string talkMsg;
    public float talkTime;


    public GUIStyle style;
    
    void Awake ()
    {
        if (!instance)
            instance = this;
        style.richText = true;
    }

    public void Talk(string m)
    {
        talkMsg = m;
        talkTime = 0.07f * m.Length + 2;
    }
    public void Talk(string m, float d)
    {
        StartCoroutine(DoTalk(m, d));
    }
    IEnumerator DoTalk(string m, float d)
    {
        yield return new WaitForSeconds(d);
        talkMsg = m;
        talkTime = 0.07f * m.Length + 2;
    }

    void Update ()
    {
        if (talkTime > 0)
            talkTime -= Time.deltaTime;
    }

    void OnGUI ()
    {
        if (Event.current.type == EventType.repaint)
        {
            int w = Ppl.instance.inventoryWeps[Ppl.instance.usingWep];
            if (!Ppl.instance.rigOverride && w >= 0 && Ppl.instance.bullets[w].all >= 0)
            {
                style.alignment = TextAnchor.MiddleRight;
                style.fontSize = (int)(Screen.height * 0.04f);
                GUI.color = Color.white;
                GUI.Label(new Rect(Screen.width*0.9f, Screen.height*0.85f, Screen.width*0.08f, Screen.height*0.1f), Ppl.instance.bullets[w].curr + "/" + Ppl.instance.bullets[w].all, style);
            }

            if (talkTime > 0)
            {
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = (int)(Screen.height*0.02f);
                GUI.color = new Color(1, 1, 1, talkTime*3);
                GUI.Label(new Rect(0, Screen.height*0.7f, Screen.width, Screen.height*0.2f), talkMsg, style);
            }
        }
    }
}