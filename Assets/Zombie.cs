using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
    public Animator anim;
    public AudioSource aud;

    public AudioSource wallAud;

    public void doWall ()
    {
        anim.Play("doWall");
        wallAud.clip = Manager.instance.effectClips[0];
        wallAud.Play();
        CancelInvoke();
        InvokeRepeating("DoRattle", 5.2f, 3.333f);
    }
    void DoRattle()
    {
        wallAud.clip = Manager.instance.effectClips[0];
        wallAud.Play();
    }
}
