using UnityEngine;

public class Flashbacker : MonoBehaviour {
    public GameObject fire;
    public Light l;
    public GameObject halo;
    public float buttonPos;

    bool canInter = false, inView = false;

    void Start()
    {
        Cam.preRendDels.Add(PreRend);
    }

    public void Begin()
    {
        l.enabled = true;
        GetComponent<Collider>().enabled = false;
        halo.SetActive(true);
        //cdTex.loop = false;
        //cdTex.Play();
    }

    public void Finish()
    {
        l.enabled = false;
        halo.SetActive(false);
        fire.SetActive(true);
    }

    void PreRend()
    {
        if (canInter)
        {
            Vector3 p = Ppl.instance.cam.WorldToViewportPoint(transform.position + new Vector3(0, buttonPos, 0));
            inView = p.z > 0 && p.x > 0 && p.y > 0 && p.x < 1 && p.y < 1;
            if (inView)
            {
                HUD.instance.SetE(p);
            }
        }
    }

    void Update ()
    {
        if (canInter && inView && Input.GetKeyDown(InKeys.Key("E")))
        {
            canInter = false;
            Begin();
            //Finish();
        }
    }

    void OnTriggerEnter ()
    {
        canInter = true;
    }

    void OnTriggerExit ()
    {
        canInter = false;
    }
}
