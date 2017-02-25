using UnityEngine;
using System;

public class Clock : MonoBehaviour {
    public Transform hr, min, sec;
    int ls;
    public AudioSource src;

	void Update () {
        DateTime t = DateTime.Now;
        hr.localRotation = Quaternion.Euler(t.Hour * 30 - 90, -90, 90);
        min.localRotation = Quaternion.Euler(t.Minute * 6 - 90, -90, 90);
        sec.localRotation = Quaternion.Euler(t.Second * 6 - 90, -90, 90);
        if (ls != t.Second) {
            ls = t.Second;
            src.Play();
        }
    }
}
