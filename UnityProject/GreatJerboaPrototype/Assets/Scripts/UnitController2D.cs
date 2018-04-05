using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class UnitController2D : MonoBehaviour {

	Rigidbody2D unitRigidBody2D;
	public Rigidbody2D Rigidbody2D { get; protected set; }

	[SerializeField]
	private Vector2 a_Velocity;

	[SerializeField]
	private Vector2 maxVelocity;
	//private Vector2 minVelocity;


	void Awake(){
		unitRigidBody2D = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		clampVelocity ();
		a_Velocity = unitRigidBody2D.velocity;
	}

	void clampVelocity(){
		Vector2 v = unitRigidBody2D.velocity;

		float x = Mathf.Clamp (v.x, -maxVelocity.x, maxVelocity.x);
		float y = Mathf.Clamp (v.y, -maxVelocity.y, maxVelocity.y);

		Vector2 t = new Vector2 (x, y);

		unitRigidBody2D.velocity = t;
	}

	public void MoveTowardMaxVelocityX(float direction, float acceleration){
		float x = Mathf.MoveTowards (
			          	unitRigidBody2D.velocity.x,
						direction * maxVelocity.x,
			          	acceleration * Time.deltaTime);

		Vector2 v = new Vector2 (x, unitRigidBody2D.velocity.y);
		unitRigidBody2D.velocity = v;
	}

	public void addImpulse(Vector2 force){
		unitRigidBody2D.AddForce (force, ForceMode2D.Impulse);
	}

	public void addForce(Vector2 force){
		unitRigidBody2D.AddForce (force, ForceMode2D.Force);
	}

	public void multiplyVelocityY(float multiplier){
		float y = unitRigidBody2D.velocity.y * multiplier;
		Vector2 v = new Vector2(unitRigidBody2D.velocity.x, y);
		unitRigidBody2D.velocity = v;
	}

	public void setClampVelocity(Vector2 clampMax){
		maxVelocity = clampMax;
	}

	public void setGravityScale(float g){
		unitRigidBody2D.gravityScale = g;
	}

	public Vector2 getVelocity(){
		return unitRigidBody2D.velocity;
	}
}
