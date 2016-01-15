using UnityEngine;
using System.Collections;

public class SirenController : MonoBehaviour {

    private PathController target;
    private AudioSource sirenClip;

    // Use this for initialization

    void Start()
    {
        target = GetComponent<PathController>();
        sirenClip = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (target == null) return;

        if (!target.patrolMode)
        {
            if (sirenClip != null && !sirenClip.isPlaying)
            {
                sirenClip.Play();
            }
        }
        else
        {
            if (sirenClip != null)
            {
                sirenClip.Stop();
            }
        }
    }
}

