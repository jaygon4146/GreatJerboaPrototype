using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnchoredObject : MonoBehaviour {

	public GameObject anchor;
	public float chainLength;

	[SerializeField]
	public Vector2 LocalToAnchor;
	[SerializeField]
	public Vector2 LocalFromAnchor;
	[SerializeField]
	private float distanceToAnchor;

	abstract public void FollowAnchor();

	public void BaseFixedUpdate(){
		CalculateDistances ();
		if (distanceToAnchor > chainLength) {
			BasePullToAnchor ();
		}
	}

	public void CalculateDistances(){
		LocalToAnchor = anchor.transform.position - transform.position;
		LocalFromAnchor = -LocalToAnchor;
		distanceToAnchor = LocalToAnchor.magnitude;
	}

	void BasePullToAnchor(){
		Vector3 clampFromAnchorOffset = Vector3.ClampMagnitude (LocalFromAnchor, chainLength);
		float z = transform.position.z;
		Vector3 tPos = anchor.transform.position + clampFromAnchorOffset;

		transform.position = new Vector3(tPos.x, tPos.y, z);
	}
}
