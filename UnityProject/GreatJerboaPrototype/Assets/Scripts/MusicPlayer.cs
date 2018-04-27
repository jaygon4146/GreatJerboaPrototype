using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	public static MusicPlayer Instance{
		get { return s_Instance; }
	}

	protected static MusicPlayer s_Instance;

	static bool playing = false;
	private AudioSource source;
	public AudioClip clip;


	void Awake(){

		if (s_Instance == null)
			s_Instance = this;

		if (!playing) {
			source = GetComponent<AudioSource> ();
			source.Play ();
			DontDestroyOnLoad (this);
			playing = true;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
