using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class JerboaAnimationManager : MonoBehaviour {

	private Animator animator;
	private AnimatorStateInfo stateInfo;

	private Vector2 CurrentVelocity;
	private Vector2 MinVelocity;
	private Vector2 MaxVelocity;

	private float HorizontalVelocity;
	private float HorizontalMagnitude;
	private float VerticalVelocity;
	private bool BeginJump;
	private bool TakeOffToAirborne;
	private bool Landed;

	private static int HorizontalHash = Animator.StringToHash ("HorizontalVelocity");
	private static int HMagnitudeHash = Animator.StringToHash ("HorizontalMagnitude");
	private static int VerticalHash = Animator.StringToHash("VerticalVelocity");
	private static int BeginJumpHash = Animator.StringToHash("BeginJump");
	private static int TakeOffAirborneHash = Animator.StringToHash("TakeOffToAirborne");
	private static int LandedHash = Animator.StringToHash("Landed");

	void Awake(){
		animator = GetComponent<Animator> ();
	}

	void FixedUpdate(){
		stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		UpdateAnimatorVariables ();
	}

	void UpdateAnimatorVariables(){
		animator.SetFloat (HorizontalHash, HorizontalVelocity);
		animator.SetFloat (VerticalHash, VerticalVelocity);

		HorizontalMagnitude = HorizontalVelocity;
		if (HorizontalMagnitude < 0)
			HorizontalMagnitude *= -1;
		animator.SetFloat (HMagnitudeHash, HorizontalMagnitude);
		////
	}

	public void PassCurrentVelocity(Vector2 current){
		CurrentVelocity = current;
	}

	public void SetMinMaxVelocity (Vector2 min, Vector2 max){
		MinVelocity = min;
		MaxVelocity = max;
	}

	public void JumpTakeOff(){
		animator.SetTrigger(BeginJumpHash);
	}

	public void TakeOffTransitionAirborne(){
		//print ("TEST STATE INFO NAME");
		if (stateInfo.IsName ("JumpTakeOff")) {
			animator.SetTrigger (TakeOffAirborneHash);
			//print ("True");
		}
		
	}

	public void FoundLandingPos(Vector2 land){
		animator.SetTrigger (LandedHash);
	}

}
