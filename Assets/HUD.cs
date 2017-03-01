using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
    public static HUD instance;
    public float talkTime;
    public Text talkUI;

    public Camera uiCam;

    public RectTransform E, F;
    public Text ET, ED, FT, FD;
    int Et = -1, Ft = -1;

    public RectTransform LT, RB, LT2, RB2; //2 for VR
    public Vector2 lt, rb;
    
    void Awake()
    {
        if (!instance)
            instance = this;
    }

    void Start () {
        if (Ppl.instance.vr)
        {
            lt = LT2.anchoredPosition;
            rb = RB2.anchoredPosition;
        }
        else
        {
            lt = LT.anchoredPosition;
            rb = RB.anchoredPosition;
        }
        LT.gameObject.SetActive(false);
        RB.gameObject.SetActive(false);
        LT2.gameObject.SetActive(false);
        RB2.gameObject.SetActive(false);
    }

    public Vector2 CamToCanvas (Vector3 p) {
        if (Ppl.instance.vr) p.y = ((p.y - 0.5f) * Screen.width / Screen.height) + 0.5f;
        return new Vector2(Mathf.Lerp(lt.x, rb.x, p.x), Mathf.Lerp(rb.y, lt.y, p.y));
    }

    public void SetE (Vector3 p, string msg = "") {
        E.gameObject.SetActive(true);
        E.anchoredPosition = CamToCanvas(p);
        ET.text = InKeys.Nm("E");
        ED.text = msg;
        Et = 1;
    }
    public void SetF(Vector3 p, string msg = "") {
        F.gameObject.SetActive(true);
        F.anchoredPosition = CamToCanvas(p);
        FT.text = InKeys.Nm("F");
        FD.text = msg;
        Ft = 1;
    }

    public void Talk(string m)
    {
        talkUI.text = m;
        talkTime = 0.07f * m.Length + 2;
    }
    public void Talk(string m, float d)
    {
        StartCoroutine(DoTalk(m, d));
    }
    IEnumerator DoTalk(string m, float d)
    {
        yield return new WaitForSeconds(d);
        talkUI.text = m;
        talkTime = 0.07f * m.Length + 2;
    }

    void Update() {
        if (talkTime > 0) {
            talkTime -= Time.deltaTime;
            talkUI.enabled = true;
            talkUI.color = new Color(1, 1, 1, talkTime*2);
        }
        else
            talkUI.enabled = false;

        if (Et >= 0) {
            Et--;
            if (Et < 0)
                E.gameObject.SetActive(false);
        }
        if (Ft >= 0) {
            Ft--;
            if (Ft < 0)
                F.gameObject.SetActive(false);
        }
    }
}