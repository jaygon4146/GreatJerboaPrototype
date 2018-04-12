using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIKTarget : MonoBehaviour {

	//private Vector2 myWorldPos;
	//private Vector2 myLocalPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//transform.localPosition += Vector3.up *0.01f;

	}

	public void SetWorldPos(Vector2 pos){
		//myWorldPos = pos;
		transform.position = pos;
	}

	public Vector2 GetWorldPos(){
		return transform.position;
	}
}
