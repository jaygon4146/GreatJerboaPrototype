using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnchoredObject : MonoBehaviour {

	public GameObject anchor;
	public Vector2 anchorPos;
	public Vector2 myPos;
	public float chainLength;
	public Rigidbody2D rb;

	[Tooltip("Specifies at what percentage of tension the chain should retract")]
	[Range(0f,1f)]
	public float chainStretch = 1f;
	private float relaxedLength; //
	public float elasticMagnitude = 0f;

	//[SerializeField]
	public Vector2 LocalToAnchor; //
	//[SerializeField]
	public Vector2 LocalFromAnchor; //
	[SerializeField]
	private float distanceToAnchor; //
	[SerializeField]
	private Vector2 directionToAnchor; //
	//private Vector2 directionFromAnchor;


	private float elasticity;
	private Vector2 elasticForce;
	//private Vector2 extendedLocalPos;
	//private Vector2 relaxedLocalPos;
	//private Vector2 extendedWorldPos;
	//private Vector2 relaxedWorldPos;

	public Vector2 clampMaxLerp = new Vector2 (1,1);
	public Vector2 clampMinLerp = new Vector2 (-1, -1);
	private Vector2 clampMaxValue;
	private Vector2 clampMinValue;

	abstract public void FollowAnchor();

	//=========================================================

	public void BaseAwake(){
		rb = GetComponent<Rigidbody2D> ();
		CalculateDistances ();
	}

	public void BaseFixedUpdate(){
		CalculateDistances ();
		SimpleClampPosition ();
	}

	public void CalculateDistances(){
		anchorPos = anchor.transform.position;
		myPos = transform.position;

		relaxedLength = chainLength * chainStretch;
		LocalToAnchor = anchor.transform.position - transform.position;
		LocalFromAnchor = -LocalToAnchor;
		distanceToAnchor = LocalToAnchor.magnitude;
		directionToAnchor = LocalToAnchor.normalized;
		//directionFromAnchor = LocalFromAnchor.normalized;

		//relaxedLocalPos = directionFromAnchor * relaxedLength;
		//relaxedWorldPos = relaxedLocalPos + anchorPos;

		//extendedLocalPos = directionFromAnchor * chainLength;
		//extendedWorldPos = extendedLocalPos + anchorPos;

		clampMaxValue = clampMaxLerp * chainLength;
		clampMinValue = clampMinLerp * chainLength;
	}

	public void SimpleClampPosition(){
		Vector3 clamp = Vector3.ClampMagnitude (LocalFromAnchor, chainLength);
		float z = transform.position.z;
		Vector3 pos = anchor.transform.position + clamp;

		transform.position = new Vector3 (pos.x, pos.y, z);
	}

	public void SimpleElasticity(){
		/*
		 * The chain that binds us to our anchor should act like a rubber band, with elasticity
		*/
		elasticForce = Vector2.zero;
		elasticity = Mathf.InverseLerp (relaxedLength, chainLength, distanceToAnchor);
		elasticForce += directionToAnchor * elasticMagnitude * elasticity;

		if (elasticity >= 1) {
			SimpleClampPosition ();
			//rb.velocity = Vector2.zero;
		}
		rb.AddForce (elasticForce, ForceMode2D.Force);
	}

	public void AdvancedClampPosition(){
		float tX = Mathf.Clamp (LocalFromAnchor.x, clampMinValue.x, clampMinValue.x);
		float tY = Mathf.Clamp (LocalFromAnchor.y, clampMinValue.y, clampMaxValue.y);

		tX = tX + anchor.transform.position.x;
		tY = tY + anchor.transform.position.y;
		float tZ = transform.position.z;

		transform.position = new Vector3 (tX, tY, tZ);
	}

	public float getElasticity(){
		return elasticity;
	}

	public Vector2 getElasticForce(){
		return elasticForce;
	}
}
