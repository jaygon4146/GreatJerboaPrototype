using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonCollider : MonoBehaviour {

	private AudioSource source;
	private Collider2D collider;

	//[SerializeField] private bool touchingPlatform = false;

	public AudioClip[] bounceClips;
	private float volumeScale = 1f;


	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource> ();
		collider = GetComponent<Collider2D> ();
	}

	void OnCollisionEnter2D(Collision2D c){
		//if (c.collider.tag == "SolidPlatform") {
			//touchingPlatform = true;

			int clip = Random.Range (0, bounceClips.Length);

			volumeScale = Random.Range (0.6f, 0.8f);
			float pitchScale = Random.Range (0.75f, 1.25f);
			source.pitch = pitchScale;
			source.PlayOneShot (bounceClips [clip], volumeScale);
		//}
	}

	void OnCollisionStay2D(Collision2D c){
		//if (c.collider.tag == "SolidPlatform") {
			//touchingPlatform = true;
		//}
	}

	void OnCollisionExit2D(Collision2D c){
		//if (c.collider.tag == "SolidPlatform") {
			//touchingPlatform = false;
		//}
	}
}
