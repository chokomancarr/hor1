using UnityEngine;

public class Flashbacker : MonoBehaviour {
    public GameObject fire;
    public Light l;
    public GameObject halo;
    public MovieTexture cdTex;

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

    void OnTriggerStay ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Begin();
            //Finish();
        }
    }
}
