using UnityEngine;
using System.Collections.Generic;

public class Cam : MonoBehaviour {
    public Ppl ppl;
    public Transform uiPivot;
    public Camera uicam;

    public delegate void OnPreRend();
    public static List<OnPreRend> preRendDels;

    void Awake () {
        preRendDels = new List<OnPreRend>();
    }
    
    /*
    void Update() {
        if (Input.GetKey(KeyCode.KeypadPlus)) {
            ppl.cam.fieldOfView += Time.deltaTime * 10;
            ppl.cam.fieldOfView = Mathf.Min(ppl.cam.fieldOfView, 160);
        }
        if (Input.GetKey(KeyCode.KeypadMinus)) {
            ppl.cam.fieldOfView -= Time.deltaTime * 10;
            ppl.cam.fieldOfView = Mathf.Max(ppl.cam.fieldOfView, 80);
        }
    }
	*/
	// Update is called once per frame
	void OnPreRender () {
        if (ppl.vr) {
            ppl.camPivot.transform.localPosition = ppl.pvPos - transform.localPosition;
            uiPivot.transform.localPosition = -uicam.transform.localPosition;
            if (!ppl.rigOverride) {
                ppl.cam.fieldOfView = Mathf.Lerp(ppl.cam.fieldOfView, 120, Time.deltaTime*5);
                ppl.rigPivot.rotation = ppl.cam.transform.rotation;
                ppl.rigPivot.Rotate(0, 180, -90);
            }
            else {
                if (ppl.preOverride < 1) {
                    ppl.cam.fieldOfView = 120 - 20*ppl.preOverride;
                    //transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.identity, Time.deltaTime * 10);
                }
                else {
                    ppl.cam.fieldOfView = 100;
                    ppl.camPivot.transform.position = ppl.camOverrideTr.position;
                    ppl.camPivot.transform.Translate(-transform.localPosition, Space.Self);
                }
            }
        }
        foreach (OnPreRend r in preRendDels)
            r.Invoke();
	}
}
