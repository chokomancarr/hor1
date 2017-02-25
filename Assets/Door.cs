using UnityEngine;

public class Door : Interactable {
    public bool open;

    public Vector3 oriPos;

    public AudioSource s;
    public AudioClip closeClip;

    void Awake ()
    {
        oriPos = rep.transform.localPosition;
    }

    protected override void OnDoAction() {
        rep.GetComponent<Collider>().enabled = false;
        open = true;
    }

    public override void Reset()
    {
        base.Reset();
        rep.GetComponent<Collider>().enabled = true;
        open = false;
    }

    public void Close()
    {
        Reset();
        rep.transform.localPosition = oriPos;
        if (closeClip)
        {
            s.clip = closeClip;
            s.Play();
        }
    }
}
