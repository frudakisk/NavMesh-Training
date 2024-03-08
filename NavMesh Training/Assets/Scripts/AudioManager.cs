using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();

    private AudioSource audioSource;
    private AudioClip lastPlayedClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomClip();
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            PlayRandomClip();
        }
    }

    /// <summary>
    /// Randomly selects a clip from clips list that was not the last clip
    /// played.
    /// </summary>
    private void PlayRandomClip()
    {
        if(clips.Count == 0)
        {
            Debug.Log("No Music!");
            return;
        }

        AudioClip randomClip = clips[Random.Range(0, clips.Count)];
        if(randomClip == lastPlayedClip && clips.Count > 1)
        {
            randomClip = clips[(clips.IndexOf(randomClip) + 1) % clips.Count];
        }

        lastPlayedClip = randomClip;
        audioSource.clip = randomClip;
        audioSource.Play();
    }
}
