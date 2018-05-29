using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour {

    private AudioSource source;

    public AudioClip cursorClip;
    public AudioClip confirmClip;
    public AudioClip cancelClip;
    public AudioClip beginLevelClip;
    
    public float volumeScale = 1f;
    public float pitchScale = 1f;

    void Awake ()
    {
        source = GetComponent<AudioSource>();
    }

    private void PlayClip(AudioClip clip)
    {

        source.pitch = pitchScale;
        source.PlayOneShot(clip, volumeScale);
    }

    public void PlayCursor()
    {
        pitchScale = Random.Range(1f, 1f);
        volumeScale = Random.Range(0.5f, 0.6f);
        PlayClip(cursorClip);
    }

    public void PlayConfirm()
    {
        pitchScale = Random.Range(0.95f, 1.05f);
        volumeScale = Random.Range(0.5f, 0.6f);
        PlayClip(confirmClip);
    }

    public void PlayCancel()
    {
        pitchScale = Random.Range(0.95f, 1.05f);
        volumeScale = Random.Range(0.5f, 0.6f);
        PlayClip(cancelClip);
    }

    public void PlayBeginLevel()
    {
        pitchScale = Random.Range(0.95f, 1.05f);
        volumeScale = Random.Range(0.5f, 0.6f);
        PlayClip(beginLevelClip);
    }

}
