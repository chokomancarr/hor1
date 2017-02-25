using UnityEngine;

public class Interactable : MonoBehaviour {
    public GameObject rep;
    Collider col;
    public int type;
    public Vector3 buttonPos;

    public bool canInter, inView;
    GUIStyle style;
    float dh;

    protected virtual void OnDoAction() { }
    public virtual void Reset()
    {
        canInter = false;
        col.enabled = true;
    }

    void Start () {
        dh = Screen.height * 0.05f;
        col = GetComponent<Collider>();
        style = new GUIStyle();
        style.fontSize = (int)(dh);
        style.normal.textColor = Color.white;
    }

    void Update ()
    {
        if (canInter && inView && Input.GetKeyDown(InKeys.Key("E")))
        {
            col.enabled = false;
            canInter = false;
            OnDoAction();
            Ppl.instance.Override(transform, rep, type, this);
        }
    }

    void OnGUI ()
    {
        if (Event.current.type == EventType.repaint && canInter)
        {
            Vector3 p = Ppl.instance.cam.WorldToScreenPoint(transform.TransformPoint(buttonPos));
            inView = p.z > 0 && p.x > 0 && p.y > 0 && p.x < Screen.width && p.y < Screen.height;
            if (inView)
            {
                GUI.DrawTexture(new Rect(p.x - dh * 0.5f, Screen.height - p.y - dh * 0.5f, dh * 0.7f, dh), Manager.instance.tex_black);
                GUI.Label(new Rect(p.x - dh * 0.5f, Screen.height - p.y - dh * 0.5f, dh, dh), InKeys.Nm("E"), style);
            }
        }
    }

    void OnTriggerEnter ()
    {
        canInter = true;
    }

    void OnTriggerExit()
    {
        canInter = false;
    }
}
