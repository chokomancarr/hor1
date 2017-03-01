using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;

public class Ppl : MonoBehaviour {
    public static Ppl instance;
    public bool vr;

    public Rigidbody rig;
    public Collider col;
    public Animator anim;
    public Camera cam;
    public Vector3 camFwd;
    public Transform camPivot;
    public Transform rigPivot;
    public Transform rigItem;
    public Transform camOverrideTr;
    public Image aimDotTr;
    public int usingWep;
    public bool canChangeWep = true;
    public Weapon wepScr;
    public GameObject[] weps;

    public int[] inventoryWeps;
    
    public List<BulletInfo> bullets;
    [System.Serializable]
    public class BulletInfo { public int curr, all, sz; }
    public bool GotBullet(int i) { return bullets[i].curr > 0 || bullets[i].sz <= 0; }
    public bool UseBullet(int i) { if (bullets[i].sz > 0 && (--bullets[i].curr) <= 0) { Reload(i); return true; } else return false; } //returns isReloaded?
    public void Reload(int i) { int rm = Mathf.Min(bullets[i].sz, bullets[i].all); bullets[i].all -= rm; bullets[i].curr += rm; }

    public LayerMask attMask;

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
        VRSettings.enabled = vr;
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

            if ((InKeys.isJoystick ? Input.GetKeyDown(KeyCode.Joystick1Button1) : Input.GetMouseButtonDown(0)) && wepScr && GotBullet(inventoryWeps[usingWep]))
            {
                if (wepScr.canFire)
                {
                    wepScr.Fire();
                    anim.Play("Fire", 0, 0);
                    anim.SetFloat("isReload", UseBullet(inventoryWeps[usingWep])? 1 : 0);
                    Att(wepScr.sz, wepScr.dmg, wepScr.delay, wepScr.dst);
                    HUD.instance.UpdateAmmo();
                }
            }

            if (canChangeWep) {
                float hr = Input.GetAxis("Dpad-Hor");
                float vr = Input.GetAxis("Dpad-Ver");
                int a = -1;
                if (hr != 0)
                    a = hr > 0 ? 2 : 0;
                if (vr != 0)
                    a = vr > 0 ? 1 : 3;
                if (a > -1 && a != usingWep) {
                    UseWep(a, true);
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

        if (rigOverride) {
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

    public void Att(float rad, float dmg, float t = 0, float dist = Mathf.Infinity) {
        StartCoroutine(DoAtt(rad, dmg, t, dist));
    }

    IEnumerator DoAtt (float rad, float dmg, float t, float dist) {
        yield return new WaitForSeconds(t);
        RaycastHit info;
        if (Physics.SphereCast(transform.TransformPoint(pvPos), rad, camFwd, out info, dist, attMask.value)) {
            DmgPart p = info.collider.gameObject.GetComponent<DmgPart>();
            if (p) {
                p.Hit(dmg, info);
            }
        }
    }

    public void Override (Transform t, GameObject r, int id, Interactable i)
    {
        StartCoroutine(DoOverride(t, r, id, i));
    }

    IEnumerator DoOverride(Transform t, GameObject r, int id, Interactable i)
    {
        if (i.one) i.one.DoSt();
        int wo = usingWep + 0;
        rigOverride = true;
        preOverride = 0;
        overrideTr = t;
        overrideReplace = r;
        preOPos = transform.position;
        preORot = transform.rotation;
        rig.isKinematic = true;
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        if (i.one) i.one.Do5();
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
        if (i.one) i.one.DoEnd();
        rigOverride = false;
        cam.transform.localRotation = Quaternion.identity;
        camDirX = transform.eulerAngles.y;
        camDirY = 0;
        UseWep(wo);
        transform.Translate(targets[id].off);
        rig.isKinematic = false;
        col.enabled = true;
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
        else if (targets[id].res)
            i.Reset();
    }

    public void UseWep (int i, bool show = false) {
        HUD.instance.ammoCurr.gameObject.SetActive(false);
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
        if (inventoryWeps[usingWep] >= 0) {
            weps[inventoryWeps[usingWep]].SetActive(true);
            wepScr = weps[inventoryWeps[usingWep]].GetComponent<Weapon>();
            if (wepScr) {
                wepScr.Refresh();
                HUD.instance.ammoCurr.gameObject.SetActive(bullets[inventoryWeps[usingWep]].sz > 0);
                HUD.instance.UpdateAmmo();
            }
            aimDotTr.gameObject.SetActive(true);
            aimDotTr.sprite = wepScr.aimDot;
        }
        else {
            wepScr = null;
            aimDotTr.gameObject.SetActive(false);
        }
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
    public Vector3 off;
    public bool res;
}