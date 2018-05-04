using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableIcon : MonoBehaviour {

	public Sprite emptySprite;
	public Sprite filledSprite;

	private Image thisImage;

	void Awake(){
		thisImage = GetComponent<Image> ();
		thisImage.sprite = emptySprite;
	}

	public void fill(){
		thisImage.sprite = filledSprite;
	}

}
