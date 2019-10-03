using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioSource source;
    // Use this for initialization
    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        source = FindObjectOfType<AudioSource>();
        source.loop = false;
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            Debug.Log("Random song num is : " + GetRandomClip());
            source.clip = GetRandomClip();
            source.Play();
        }
    }
}
