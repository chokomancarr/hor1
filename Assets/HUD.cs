using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
    public static HUD instance;
    public float talkTime;
    public Text talkUI;

    public Camera uiCam;

    public RectTransform E, F;
    public Text ET, FT;
    int Et = -1, Ft = -1;

    public RectTransform LT, RB;
    public Vector2 lt, rb;

    void Start () {
        lt = LT.anchoredPosition;
        rb = RB.anchoredPosition;
        LT.gameObject.SetActive(false);
        RB.gameObject.SetActive(false);
    }

    public Vector2 CamToCanvas (Vector3 p) {
        p.y = ((p.y - 0.5f) * Screen.width / Screen.height) + 0.5f;
        return new Vector2(Mathf.Lerp(lt.x, rb.x, p.x), Mathf.Lerp(rb.y, lt.y, p.y));
    }

    public void SetE (Vector3 p) {
        E.gameObject.SetActive(true);
        E.anchoredPosition = CamToCanvas(p);
        ET.text = InKeys.Nm("E");
        Et = 1;
    }
    public void SetF(Vector3 p) {
        F.gameObject.SetActive(true);
        F.anchoredPosition = CamToCanvas(p);
        FT.text = InKeys.Nm("F");
        Ft = 1;
    }

    void Awake ()
    {
        if (!instance)
            instance = this;

        Talk("Hello from the other side!");
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