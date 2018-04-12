using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;

[RequireComponent(typeof(Animator))]

public class JerboaAnimationManager : MonoBehaviour {

	private Animator animator;
	private AnimatorStateInfo stateInfo;
	[SerializeField]
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

	private static int JumpTakeOffState = Animator.StringToHash ("JumpTakeOff");

	public GameObject FrontFootTarget;
	public GameObject BackFootTarget;
	public FeetAnchor FeetAnchoredObj;
	public bool AnchorFeetY = false;
	public bool isJumping = false;
	private VisibleBool VisIsJumping = new VisibleBool();

	public bool debugging = false;

	void Awake(){
		animator = GetComponent<Animator> ();

		if (debugging)
			VisIsJumping.turnOn ();

	}

	void FixedUpdate(){
		stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		UpdateAnimatorVariables ();

		if (isJumping) {
			//AdjustFeetVertical ();
		}

		//FeetAnchoredObj.SetPullingToGround (AnchorFeetY);
		FeetAnchoredObj.SetIsJumping (isJumping);
		FeetAnchoredObj.PassCurrentVelocity (CurrentVelocity);


		if (debugging) {
			Vector2 pos = transform.position;
			VisIsJumping.drawOrigin = pos + new Vector2 (4, 2);
			VisIsJumping.updateValue (isJumping);
			VisIsJumping.drawBoolLine ();
		}

		/*
		if (stateInfo.shortNameHash == JumpTakeOffState) {
			print ("ShortNameHash; JumpTakeOffState");
		}
		*/

	}

	void UpdateAnimatorVariables(){

		HorizontalVelocity = CurrentVelocity.x;
		HorizontalMagnitude = HorizontalVelocity;
		if (HorizontalMagnitude < 0) {			
			HorizontalMagnitude *= -1;
		}
		animator.SetFloat (HMagnitudeHash, HorizontalMagnitude);
		animator.SetFloat (HorizontalHash, HorizontalVelocity);


		float y = 0;
		if (CurrentVelocity.y > 0) { //If moving upward
			y = CurrentVelocity.y / MaxVelocity.y;
		} else { //Otherwise, we are moving downward
			y = -(CurrentVelocity.y / MinVelocity.y);
		}
		VerticalVelocity = y;
		animator.SetFloat (VerticalHash, VerticalVelocity);

	}

	void AdjustFeetVertical(){
		Vector2 f = new Vector2 (FrontFootTarget.transform.position.x, FeetAnchoredObj.transform.position.y);
		Vector2 b = new Vector2 (BackFootTarget.transform.position.x, FeetAnchoredObj.transform.position.y);

		FrontFootTarget.transform.position = f;
		BackFootTarget.transform.position = b;
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
		if (stateInfo.shortNameHash == JumpTakeOffState) {
			animator.SetTrigger (TakeOffAirborneHash);
		}	
	}

	public void JumpCancelEarly(){
		FeetAnchoredObj.BeginRetractingUp ();
		TakeOffTransitionAirborne ();
	}

	public void FoundLandingPos(Vector2 land){
		//Fix Landed Trigger
		animator.SetTrigger (LandedHash);
	}

}
