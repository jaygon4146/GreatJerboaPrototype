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
	public float springJumpMultiplier = 3f;

	[SerializeField]
	private float jumpInitialVelocity;
	[SerializeField]
	private float jumpMaxVelocity;
	[SerializeField]
	private float jumpInitialGravity;
	[SerializeField]
	private Vector2 jumpVector;
	#endregion
	//==============================================
	#region MovementVariables
	public float topSpeed = 10f;
	public float acceleration = 1f;
	public float deceleration = 1f;
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
		jumpMaxVelocity = jumpInitialVelocity * jumpFallingMultiplier;
	}

	public Vector2 getJumpVector(){
		return jumpVector;
	}

	public Vector2 getSpringJumpVector(){
		return jumpVector * springJumpMultiplier;
	}

	public float getJumpInitialGravity(){
		return jumpInitialGravity;
	}
	public float getJumpInitialVelocity(){
		return jumpInitialVelocity;
	}

	public float getJumpMaxVelocity(){
		return jumpMaxVelocity;
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
