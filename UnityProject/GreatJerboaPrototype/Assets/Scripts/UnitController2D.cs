using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class UnitController2D : MonoBehaviour {

	Rigidbody2D unitRigidBody2D;
	public Rigidbody2D Rigidbody2D { get; protected set; }

	[SerializeField]
	private Vector2 a_Velocity;


	void Awake(){
		unitRigidBody2D = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		a_Velocity = unitRigidBody2D.velocity;
	}

	public void addImpulse(Vector2 force){
		unitRigidBody2D.AddForce (force, ForceMode2D.Impulse);
	}

	public void setGravityScale(float g){
		unitRigidBody2D.gravityScale = g;
	}

	public Vector2 getVelocity(){
		return unitRigidBody2D.velocity;
	}
}
