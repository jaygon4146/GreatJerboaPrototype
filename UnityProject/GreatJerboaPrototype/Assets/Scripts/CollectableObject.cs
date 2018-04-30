using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour {

	[SerializeField] private bool touchingPlayerCharacter = false;
	[SerializeField] private bool HasBeenTriggered = false;

	private SpriteRenderer renderer;
	private Animator anim;
	private AudioSource source;
	public AudioClip clip;

	public Sprite basicSprite;
	public Sprite wingSprite;
	public Sprite ghostSprite;

	public int minCount = 50;
	public int maxCount = 200;
	[SerializeField] private int flickerCountdown;
	public int minOpen = 5;
	public int maxOpen = 20;
	[SerializeField] bool isOpen = false;



	void Awake(){
		renderer = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();

	}

	void Update(){
		if (!HasBeenTriggered) {
			flickerCountdown--;

			if (flickerCountdown <= 0) {
				if (isOpen) {
					renderer.sprite = basicSprite;
					isOpen = false;
					flickerCountdown = Random.Range (minCount, maxCount);
				} else {//not Open
					renderer.sprite = wingSprite;
					isOpen = true;
					flickerCountdown = Random.Range (minOpen, maxOpen);
				}
			}
		}
	}


	public bool wasCollected(){
		return HasBeenTriggered;
	}

	void OnTriggerEnter2D(Collider2D c){
		if (!HasBeenTriggered)
			source.PlayOneShot (clip);
		
		touchingPlayerCharacter = true;
		HasBeenTriggered = true;
		//renderer.color = Color.blue;
		renderer.sprite = ghostSprite;
		//print ("CollectableEnter");
	}

	void OnTriggerStay2D(Collider2D c){
		touchingPlayerCharacter = true;
		//print ("CollectableStay");
	}

	void OnTriggerExit2D(Collider2D c){
		touchingPlayerCharacter = false;
		//print ("CollectableExit");
	}
}
