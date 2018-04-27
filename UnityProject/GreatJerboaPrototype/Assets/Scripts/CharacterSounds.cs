using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour {

	private AudioSource source;

	public AudioClip jumpClip;
	public AudioClip footClip;
	public AudioClip bumpClip;

	public float volumeScale = 1f;
	public float pitchScale = 1f;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	private void PlayClip(AudioClip clip){
		pitchScale = Random.Range (0.75f, 1.25f);

		source.pitch = pitchScale;
		source.PlayOneShot (clip, volumeScale);
	}

	public void PlayJump(){
		PlayClip (jumpClip);
	}

	public void PlayFootStep(){
		PlayClip (footClip);

	}

	public void PlayBump(){
		PlayClip (bumpClip);
	}

}
