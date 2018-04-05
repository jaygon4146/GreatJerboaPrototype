﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController2D))]
[RequireComponent(typeof(PhysicsForces))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerCharacter : MonoBehaviour {

	#region GameComponents
	protected UnitController2D 	PCUnitController2D;
	protected PhysicsForces 	PCPhysicsForces;
	private Collider2D Collider;
	#endregion

	#region PlayerStatus
	[SerializeField]
	private bool touchingPlatform;
	[SerializeField]
	private bool canJump;
	private bool canCancelJump;
	#endregion




	void Awake(){
		PCUnitController2D = GetComponent<UnitController2D> ();
		PCPhysicsForces = GetComponent<PhysicsForces> ();
		Collider = GetComponent<Collider2D> ();
	}

	void FixedUpdate(){

		JumpAction ();
		MoveAction ();
		HorizontalAction ();
		VerticalAction ();

		Vector2 maxV = new Vector2 (PCPhysicsForces.topSpeed, PCPhysicsForces.getJumpMaxVelocity());
		PCUnitController2D.setClampVelocity (maxV);

		if (!PCPhysicsForces.applyGravity) {
			PCUnitController2D.setGravityScale (0);
		}
	}

	void JumpAction(){
		canJump = touchingPlatform;

		if (PlayerInput.Instance.Jump.Down && canJump && PCPhysicsForces.applyGravity) {
			//print ("PlayerCharacter.JumpAction()");
			PCPhysicsForces.CalculateJumpForces ();
			PCUnitController2D.setGravityScale (PCPhysicsForces.getJumpInitialGravity());
			Vector2 jumpVector = PCPhysicsForces.getJumpVector ();
			PCUnitController2D.addImpulse (jumpVector);
			canCancelJump = true;
		}

		if (PlayerInput.Instance.Jump.Up && canCancelJump) {
			PCUnitController2D.multiplyVelocityY (0.5f);
			canCancelJump = false;
		}
	}

	void MoveAction(){
		float inputDead = 0.01f;
		bool activeInput = false;
		float horzInput = PlayerInput.Instance.Horizontal.Value;
		if (horzInput < -inputDead || horzInput > inputDead) {
			activeInput = true;
		}

		float direction = activeInput ? horzInput : 0f;
		float accel = activeInput ? PCPhysicsForces.acceleration : PCPhysicsForces.deceleration;

		PCUnitController2D.MoveTowardMaxVelocityX (direction, accel);
		//PCUnitController2D.addImpulse (force);	
	}

	void HorizontalAction(){


	}

	void VerticalAction(){
		float yVelocity = PCUnitController2D.getVelocity ().y;

		if (yVelocity < 0) {
			PCUnitController2D.setGravityScale (PCPhysicsForces.getFallingGravity());
			canCancelJump = false;
		}
	}

	#region ColliderEvents
	void OnCollisionEnter2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = true;
		}
	}

	void OnCollisionStay2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = true;
		}
	}

	void OnCollisionExit2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = false;
		}
	}
	#endregion


}