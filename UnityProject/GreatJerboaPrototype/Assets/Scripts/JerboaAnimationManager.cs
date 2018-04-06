using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class JerboaAnimationManager : MonoBehaviour {

	private Animator animator;

	void Awake(){
		animator = GetComponent<Animator> ();
	}
}
