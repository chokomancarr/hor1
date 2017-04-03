﻿using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {
    public GameObject rep;
    Collider col;
    public int type;
    public Vector3 buttonPos;

    public bool canInter, inView;
    public OnE one;

    GUIStyle style;
    float dh;

    protected virtual void OnDoAction() { }
    public virtual void Reset()
    {
        canInter = false;
        col.enabled = true;
    }

    void Start () {
        Cam.preRendDels.Add(PreRend);
        dh = Screen.height * 0.05f;
        col = GetComponent<Collider>();
        style = new GUIStyle();
        style.fontSize = (int)(dh);
        style.normal.textColor = Color.white;
    }

    void Update ()
    {
        if (canInter && Input.GetKeyDown(InKeys.Key("E")))
        {
            col.enabled = false;
            canInter = false;
			if (Ppl.instance.targets [type].item == ItemType.Undefined || Ppl.instance.inventoryItems.Contains (Ppl.instance.targets [type].item)) {
				OnDoAction ();
				Ppl.instance.Override (transform, rep, type, this);
			}
			else StartCoroutine (NeedItem(Ppl.instance.targets [type].noItemTalk));
        }
    }

	IEnumerator NeedItem (string s) {
		HUD.instance.Talk (s);
		yield return new WaitForSeconds (3);
		Reset ();
	}

    void PreRend ()
    {
        if (canInter)
        {
            Vector3 p = Ppl.instance.cam.WorldToViewportPoint(transform.TransformPoint(buttonPos));
            inView = p.z > 0 && p.x > 0 && p.y > 0 && p.x < 1 && p.y < 1;
            if (inView)
            {
                HUD.instance.SetE(p);
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