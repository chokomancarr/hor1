using UnityEngine;

public class PplAud : MonoBehaviour {

    public AudioSource src;

    public AudioClip[] footsteps;
    public AudioClip[] actions;
    public AudioClip[] actionsNoWep;

    public void Step()
    {
        src.clip = footsteps[Random.Range(0, footsteps.Length)];
        src.Play();
    }

    public void Action(int i, bool wep)
    {
        src.clip = wep ? actions[i] : actionsNoWep[i];
        src.Play();
    }
}
