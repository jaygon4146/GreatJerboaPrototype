using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController2D))]
[RequireComponent(typeof(PhysicsForces))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerCharacter : MonoBehaviour {

	#region GameComponents
	protected UnitController2D 	PCUnitController2D;
	protected PhysicsForces 	PCPhysicsForces;
	#endregion

	#region PlayerStatus
	bool canJump = true;
	#endregion


	void Awake(){
		//print ("PlayerCharacter.Awake()");

		PCUnitController2D = GetComponent<UnitController2D> ();
		PCPhysicsForces = GetComponent<PhysicsForces> ();
	}


	void FixedUpdate(){
		JumpAction ();
		MoveAction ();
		HorizontalAction ();
		VerticalAction ();

		if (!PCPhysicsForces.applyGravity) {
			PCUnitController2D.setGravityScale (0);
		}
	}

	void JumpAction(){
		if (PlayerInput.Instance.Jump.Down && canJump && PCPhysicsForces.applyGravity) {
			//print ("PlayerCharacter.JumpAction()");
			PCPhysicsForces.CalculateJumpForces ();
			PCUnitController2D.setGravityScale (PCPhysicsForces.getJumpInitialGravity());
			Vector2 jumpVector = PCPhysicsForces.getJumpVector ();
			PCUnitController2D.addImpulse (jumpVector);
		}

		if (PlayerInput.Instance.Jump.Up) {

		}
	}

	void MoveAction(){

	}


	void HorizontalAction(){

	}

	void VerticalAction(){
		float yVelocity = PCUnitController2D.getVelocity ().y;

		if (yVelocity < 0) {
			PCUnitController2D.setGravityScale (PCPhysicsForces.getFallingGravity());
		}
	}

}
