using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsForces : MonoBehaviour {

	//==============================================
	#region Jumping Variables
	public bool applyGravity = true;

	public float jumpHeight = 10f;
	public float jumpTime = 10f;
	public float jumpFallingMultiplier = 2f;

	[SerializeField]
	private float jumpInitialVelocity;
	[SerializeField]
	private float jumpInitialGravity;
	[SerializeField]
	private Vector2 jumpVector;//{ public get; private set; }
	#endregion
	//==============================================
	#region MovementVariables
	public float topSpeed;
	public float acceleration;
	public float deceleration;
	#endregion
	//==============================================




	void Awake(){
		CalculateJumpForces ();
	}

	//==============================================
	#region Jumping Functions
	public void CalculateJumpForces(){
		jumpInitialVelocity = (2 * jumpHeight) / (jumpTime);
		jumpInitialGravity = -(-2 * jumpHeight) / (jumpTime * jumpTime);
		jumpVector = Vector2.up * jumpInitialVelocity;
	}

	public Vector2 getJumpVector(){
		return jumpVector;
	}

	public float getJumpInitialGravity(){
		return jumpInitialGravity;
	}

	public float getFallingGravity(){
		return jumpInitialGravity * jumpFallingMultiplier;
	}
	#endregion
	//==============================================
	#region Movement Functions

	#endregion
	//==============================================
}
