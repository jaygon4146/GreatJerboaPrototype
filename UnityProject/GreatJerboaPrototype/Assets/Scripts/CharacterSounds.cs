using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour {

	private AudioSource source;
	public AudioSource balloonSource;

	public AudioClip jumpClip;
	public AudioClip footClip;
	public AudioClip bumpClip;


	public AudioClip chargeClip;
	public AudioClip warmUpJumpClip;
	public AudioClip squeezeClip;
	public AudioClip bounceClip;
	public AudioClip springJumpClip;

	public float volumeScale = 1f;
	public float pitchScale = 1f;

	private bool charging = false;
	private bool wasCharging = false;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	private void PlayClip(AudioClip clip){
		pitchScale = Random.Range (0.85f, 1.15f);

		source.pitch = pitchScale;
		source.PlayOneShot (clip, volumeScale);
	}	

	private void PlayBalloonClip(AudioClip clip){
		pitchScale = Random.Range (0.85f, 1.15f);

		balloonSource.pitch = pitchScale;
		balloonSource.PlayOneShot (clip, volumeScale);
	}

	void Update(){

		if (charging && !wasCharging) {
			volumeScale = Random.Range (0.3f, 0.5f);
			PlayBalloonClip (chargeClip);
		}

		wasCharging = charging;

	}

	public void PlayJump(){
		volumeScale = Random.Range (0.5f, 0.75f);
		PlayClip (jumpClip);
	}

	public void setCharging(bool isOn){
		volumeScale = Random.Range (0.5f, 0.8f);
		charging = isOn;
	}

	public void PlayWarmUp(){
		volumeScale = Random.Range (0.5f, 0.8f);
		PlayBalloonClip (warmUpJumpClip);
		//PlayClip (squeezeClip);
	}
	public void PlaySqueeze(){
		volumeScale = Random.Range (0.5f, 0.75f);
		//PlayBalloonClip (squeezeClip);
	}

	public void PlaySpringJump(){
		volumeScale = Random.Range (0.8f, 1f);
		//PlayClip (springJumpClip);
		PlayBalloonClip (bounceClip);
	}

	public void PlayFootStep(){
		volumeScale = Random.Range (0.4f, 0.5f);
		PlayClip (footClip);

	}

	public void PlayBump(){
		volumeScale = Random.Range (0.5f, 0.8f);
		PlayClip (bumpClip);
	}

}
