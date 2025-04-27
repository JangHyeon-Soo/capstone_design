using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("Footstep")]
    public AudioSource AS_footstep;
    public AudioClip footstepClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AS_footstep = GetComponent<AudioSource>();
        AS_footstep.clip = footstepClip;
    }

  
    public void Footstep()
    {
        if (GetComponent<PlayerController>().isMove && !AS_footstep.isPlaying && GetComponent<PlayerController>().isGrounded)
            AS_footstep.PlayOneShot(footstepClip, 3f);
    }
}
