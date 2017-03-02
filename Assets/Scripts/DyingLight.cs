using UnityEngine;
using System.Collections;

public class DyingLight : MonoBehaviour {
    public AudioSource aud;
    public Light l;
    public Renderer r;
    Material mat;

    void Start ()
    {
        mat = r.material;
        StartCoroutine(DoFlicker());
    }

    IEnumerator DoFlicker ()
    {
        aud.Play();
        l.enabled = false;
        mat.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.1f);
        l.enabled = true;
        mat.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
        StartCoroutine(DoFlicker());
    }
}
