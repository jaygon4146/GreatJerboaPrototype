using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonRenderer : MonoBehaviour {
    
    int numberOfBalloons = 20;

    public FloatyBalloon floatyBalloon;
    private List<FloatyBalloon> balloons = new List<FloatyBalloon>();

    private void Awake()
    {
        PopluateBalloons();
    }

    private void PopluateBalloons()
    {
        for (int i = 0; i < numberOfBalloons; i++)
        {
            FloatyBalloon balloon = (FloatyBalloon)Instantiate(
                floatyBalloon, transform
                );
            balloons.Add(balloon);
        }
        for (int i = 0; i < balloons.Count; i++)
        {
            balloons[i].Reset();
        }
    }


    private void FixedUpdate()
    {
        
    }



}
