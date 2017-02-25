using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;

public class Ppl : MonoBehaviour {
    public static Ppl instance;
    public bool vr;

    public Rigidbody rig;
    public Animator anim;
    public Camera cam;
    public Transform camPivot;
    public Transform rigPivot;
    public Transform rigItem;
    public Transform camOverrideTr;
    public int usingWep;
    public bool canChangeWep = true;
    public Weapon wepScr;
    public GameObject[] weps;

    public int[] inventoryWeps;
    
    public List<BulletInfo> bullets;
    [System.Serializable]
    public struct BulletInfo { public int curr, all; }

    public List<int> items;

    public Vector3 pvPos;
    public float preOverride;
    public Vector3 preOPos;
    public Quaternion preORot;
    public bool rigOverride;
    public float overrideTime;
    public int overrideId;
    public Transform overrideTr;
    public GameObject overrideReplace;
    public OverrideTarget[] targets;

    public float v, sd;

    public float mx, my;
    public float camDirX, camDirY;

    public float cy;
    public float dc;
    public bool crouch;
    public float br;

    float stepDist = 0;
    float runDist = 0;
    public PplAud aud;

    void Awake()
    {
        instance = this;
    }

    void Start() {
        cy = cam.transform.localPosition.y;
        UseWep(0, true);
        camDirX = transform.eulerAngles.y;
        camDirY = cam.transform.localEulerAngles.x;
        runDist = 5;
        pvPos = camPivot.transform.localPosition;
    }

    void Update()
    {
        runDist = Mathf.MoveTowards(runDist, 5, Time.deltaTime);
        if (!rigOverride)
        {
            bool run = Input.GetKey(InKeys.Key("Shift"));
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 vv = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
                rig.velocity = Vector3.ClampMagnitude(vv, 1) * (run? v*(1f+Mathf.Clamp(runDist*2, 0, 1)*0.5f) : v) + transform.up * rig.velocity.y;
                rig.constraints = RigidbodyConstraints.FreezeRotation;
                stepDist += (run ? (1f + Mathf.Clamp(runDist * 2, 0, 1) * 0.5f) : 1f) * Time.deltaTime;
                if (run)
                    runDist -= 2 * Time.deltaTime;
                if (stepDist > sd)
                {
                    aud.Step();
                    stepDist = 0;
                }
            }
            else
            {
                rig.constraints = ~RigidbodyConstraints.FreezePositionY;
                if (stepDist > 0.5f*sd)
                {
                    aud.Step();
                    stepDist = 0;
                }
            }

            //if (Input.GetKeyDown(InKeys.Key("C")))
            //    crouch = !crouch;

            if ((InKeys.isJoystick ? Input.GetKeyDown(KeyCode.Joystick1Button1) : Input.GetMouseButtonDown(0)) && wepScr)
            {
                if (wepScr.canFire)
                {
                    wepScr.Fire();
                    anim.Play("Fire", 0, 0);
                }
            }

            if (canChangeWep)
            {
                for (int a = 0; a < 4; a++)
                {
                    if (a != usingWep && Input.GetKeyDown(KeyCode.Alpha1 + a))
                    {
                        UseWep(a, true);
                        break;
                    }
                }
            }

            dc = Mathf.Lerp(dc, crouch ? 0.6f : 1, Time.deltaTime * 10);

            camDirX += mx * Input.GetAxis("MX");
            camDirY -= my * Input.GetAxis("MY");

            camDirX = Mathf.Repeat(camDirX, 360f);
            camDirY = Mathf.Clamp(camDirY, -70f, 70f);

            cam.transform.localEulerAngles = new Vector3(camDirY, 0, 0);
            transform.eulerAngles = new Vector3(0, camDirX, 0);

            cam.transform.localPosition = new Vector3(0, cy * dc + Mathf.Sin(Time.time * 1.5f) * br, 0);
        }
    }

    void LateUpdate() {
        if (!vr) {
            if (!rigOverride) {
                rigPivot.rotation = cam.transform.rotation;
                rigPivot.Rotate(0, 180, -90);
            }
            else {
                if (preOverride < 1) {
                    transform.position = Vector3.Lerp(preOPos, overrideTr.position, preOverride);
                    cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.identity, Time.deltaTime * 10);
                    transform.rotation = Quaternion.Lerp(preORot, overrideTr.rotation, preOverride);
                    rigPivot.localRotation = Quaternion.Lerp(rigPivot.localRotation, Quaternion.Euler(0, 90, 0), preOverride);
                    preOverride += Time.deltaTime * 2;
                    if (preOverride >= 0)
                        UseWep(-1);
                }
                else {
                    transform.position = overrideTr.position;
                    transform.rotation = overrideTr.rotation;
                    rigPivot.localRotation = Quaternion.Euler(0, 90, 0);

                    cam.transform.position = camOverrideTr.position;
                    cam.transform.rotation = camOverrideTr.rotation;
                    cam.transform.Rotate(0, -90, -90);
                }
            }
        }

        if (!rigOverride) {

        }
        else {
            if (preOverride < 1) {
                transform.position = Vector3.Lerp(preOPos, overrideTr.position, preOverride);
                transform.rotation = Quaternion.Lerp(preORot, overrideTr.rotation, preOverride);
                preOverride += Time.deltaTime * 2;
                if (preOverride >= 0)
                    UseWep(-1);
            }
            else {
                transform.position = overrideTr.position;
                transform.rotation = overrideTr.rotation;
                rigPivot.localRotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }

public void Override (Transform t, GameObject r, int id, Interactable i)
    {
        StartCoroutine(DoOverride(t, r, id, i));
    }

    IEnumerator DoOverride(Transform t, GameObject r, int id, Interactable i)
    {
        int wo = usingWep + 0;
        rigOverride = true;
        preOverride = 0;
        overrideTr = t;
        overrideReplace = r;
        preOPos = transform.position;
        preORot = transform.rotation;
        yield return new WaitForSeconds(0.5f);
        overrideId = id;
        overrideReplace.SetActive(false);
        targets[id].o.SetActive(true);
        UseWep(-1);
        bool gotWep = false;
        if (targets[id].wep > -1)
        {
            foreach (int w in inventoryWeps)
            {
                if (w == targets[id].wep)
                {
                    weps[targets[id].wep].SetActive(true);
                    gotWep = true;
                    break;
                }
            }
        }
        else gotWep = true;
        aud.Action(id, gotWep);
        anim.SetFloat("actionType", gotWep? id : id + 50);
        anim.Play("Action");
        overrideTime = gotWep? targets[id].clip.length : targets[id].noWepClip.length;
        yield return new WaitForSeconds(overrideTime);
        yield return new WaitForEndOfFrame();
        if (targets[id].wep > -1 && gotWep)
        {
            weps[targets[id].wep].SetActive(false);
        }
        overrideReplace.transform.position = targets[id].o.transform.position;
        overrideReplace.transform.rotation = targets[id].o.transform.rotation;
        overrideReplace.SetActive(true);
        if (targets[id].o)
            targets[id].o.SetActive(false);
        yield return new WaitForSeconds(Time.deltaTime*2);
        rigOverride = false;
        cam.transform.localRotation = Quaternion.identity;
        camDirX = transform.eulerAngles.y;
        camDirY = 0;
        UseWep(wo);
        if (!gotWep)
        {
            i.Reset();
            switch (id)
            {
                case 2:
                    HUD.instance.Talk("I might be able to prise it open with something.");
                    break;
            }
        }
    }

    public void UseWep (int i, bool show = false)
    {
        if (i < 0)
        {
            if (inventoryWeps[usingWep] >= 0)
                weps[inventoryWeps[usingWep]].SetActive(false);
            usingWep = 0;
            anim.SetFloat("wepType", 0);
            return;
        }
        //if (inventoryWeps[usingWep] == inventoryWeps[i])
        //    return;
        if (inventoryWeps[usingWep] >= 0)
            weps[inventoryWeps[usingWep]].SetActive(false);
        usingWep = i;
        if (inventoryWeps[usingWep] >= 0)
        {
            weps[inventoryWeps[usingWep]].SetActive(true);
            wepScr = weps[inventoryWeps[usingWep]].GetComponent<Weapon>();
        }
        else
            wepScr = null;
        anim.SetFloat("wepType", inventoryWeps[usingWep]+1);
        if (show)
            anim.Play("Check", 0, 0);
        else
            anim.Play("WepIdle");
    }
}

[System.Serializable]
public class OverrideTarget
{
    public GameObject o;
    public AnimationClip clip;
    public int wep;
    public AnimationClip noWepClip;
}