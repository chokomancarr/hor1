using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
    public Animator anim;
    public AudioSource aud;

    public AudioSource wallAud;
    public AudioLowPassFilter wallAudF;

    public void doWall ()
    {
        anim.Play("doWall");
        wallAudF.cutoffFrequency = 8000;
        wallAud.clip = Manager.instance.effectClips[0];
        wallAud.Play();
        CancelInvoke();
        InvokeRepeating("DoRattle", 5.2f, 3.333f);
    }
    void DoRattle()
    {
        wallAudF.cutoffFrequency = 8000 - Mathf.Min(Vector3.Distance(wallAud.transform.position, Ppl.instance.transform.position), 6)*1000;
        wallAud.clip = Manager.instance.effectClips[0];
        wallAud.Play();
    }
}
