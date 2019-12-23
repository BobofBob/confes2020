using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAudio : MonoBehaviour
{
    private float WaitTime = 0;
    private bool tri = true;
    public static int MusicNumber = 0;
    private AudioSource MusicAudio;
    [SerializeField]
    private AudioClip[] audio;
    // Start is called before the first frame update
    void Start()
    {
        MusicAudio = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        WaitTime += Time.deltaTime;
        if (WaitTime > 2.85f && tri)
        {
            MusicAudio.clip = audio[MusicNumber];
            MusicAudio.Play();
            tri = false;
        }

    }
}
