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
	private float DistanceToGround;
	private float iLerpDistanceToGround;


	private static int HorizontalHash = Animator.StringToHash ("HorizontalVelocity");
	private static int HMagnitudeHash = Animator.StringToHash ("HorizontalMagnitude");
	private static int VerticalHash = Animator.StringToHash("VerticalVelocity");
	private static int BeginJumpHash = Animator.StringToHash("BeginJump");
	private static int TakeOffAirborneHash = Animator.StringToHash("TakeOffToAirborne");
	private static int LandedHash = Animator.StringToHash("Landed");
	private static int LandingBlendHash = Animator.StringToHash("LandingBlend");

	private static int JumpTakeOffState = Animator.StringToHash ("JumpTakeOff");
	private static int AirborneTreeState = Animator.StringToHash ("AirborneTree");
	private static int LandingTreeState = Animator.StringToHash ("LandingTree");

	//public GameObject FrontFootTarget;
	//public GameObject BackFootTarget;
	public FootIKTarget FFIKTarget;
	public FootIKTarget BFIKTarget;

	public FeetAnchor FeetAnchoredObj;
	public bool AnchorFeetY = false;
	public bool isJumping = false;

	public bool debugging = false;

	private VisibleBool VisIsJumping = new VisibleBool();
	private VisibleVector2 FrontFootDebug = new VisibleVector2 ();
	private VisibleVector2 BackFootDebug = new VisibleVector2 ();

	void Awake(){
		animator = GetComponent<Animator> ();


		if (debugging) {
			VisIsJumping.turnOn ();
			FrontFootDebug.drawColor = Color.red;
			FrontFootDebug.turnOn ();
			BackFootDebug.drawColor = Color.cyan;
			BackFootDebug.turnOn();
		}
	}

	void FixedUpdate(){
		stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		UpdateAnimatorVariables ();

		FeetAnchoredObj.SetIsJumping (isJumping);
		FeetAnchoredObj.PassCurrentVelocity (CurrentVelocity);

		if (isJumping) {
			AdjustFeetVertical ();
			if (FeetAnchoredObj.isExtendingDown () && FeetAnchoredObj.isTouchingGround()) {
				TouchDown ();
			}
		}

		if (!isJumping) {
			AdjustFeetVertical ();
		}

		float cLength = FeetAnchoredObj.getChainLength ();
		iLerpDistanceToGround = DistanceToGround / cLength;
		if (stateInfo.shortNameHash == LandingTreeState) {
			//print ("shortNameHash = LandingTreeState");
		}

		if (debugging) {
			Vector2 pos = transform.position;
			VisIsJumping.drawOrigin = pos + new Vector2 (4, 2);
			VisIsJumping.updateValue (isJumping);
			VisIsJumping.drawBoolLine ();

			FrontFootDebug.drawDebugLine ();
			BackFootDebug.drawDebugLine ();
		}
	}

	void UpdateAnimatorVariables(){

		HorizontalVelocity = CurrentVelocity.x;
		HorizontalMagnitude = HorizontalVelocity;
		if (HorizontalMagnitude < 0) {			
			HorizontalMagnitude *= -1;
		}
		animator.SetFloat (HMagnitudeHash, HorizontalMagnitude);
		animator.SetFloat (HorizontalHash, HorizontalVelocity);
		animator.SetFloat (LandingBlendHash, iLerpDistanceToGround);

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
		Vector2 f = new Vector2 (FFIKTarget.transform.position.x, FeetAnchoredObj.transform.position.y);
		Vector2 b = new Vector2 (BFIKTarget.transform.position.x, FeetAnchoredObj.transform.position.y);

		FFIKTarget.SetWorldPos(f);
		BFIKTarget.SetWorldPos (b);

		Vector2 pos = transform.position;
		FrontFootDebug.updateVectors (pos + Vector2.left, f);
		BackFootDebug.updateVectors (pos + Vector2.right, b);
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

	public void TouchDown(){
		if (stateInfo.shortNameHash == AirborneTreeState) {
			//print ("shortNameHash = AirborneTreeState");
			animator.SetTrigger (LandedHash);
		}
	}

	public bool NeedsDistanceToGround(){

		bool r = false;

		if (stateInfo.shortNameHash == LandingTreeState) {
			//print ("shortNameHash = LandingTreeState");
			r = true;
		}

		return r;
	}

	public void SetDistanceToGround(float d){
		DistanceToGround = d;
	}
}
