using UnityEngine;

public class Cam : MonoBehaviour {
    public Ppl ppl;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnPreRender () {
        if (ppl.vr) {
            ppl.camPivot.transform.localPosition = ppl.pvPos - transform.localPosition;
            if (!ppl.rigOverride) {
                ppl.rigPivot.rotation = ppl.cam.transform.rotation;
                ppl.rigPivot.Rotate(0, 180, -90);
            }
            else {
                if (ppl.preOverride < 1) {
                    //cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.identity, Time.deltaTime * 10);
                }
                else {
                    ppl.camPivot.transform.position = ppl.camOverrideTr.position - transform.localPosition;
                    //cam.transform.rotation = camOverrideTr.rotation;
                    //cam.transform.Rotate(0, -90, -90);
                }
            }
        }
	}
}
