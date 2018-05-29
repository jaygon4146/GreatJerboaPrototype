using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyBalloon : MonoBehaviour {

    private Color myColor;
    private SpriteRenderer renderer;
    private float vertSpeed;
    private float horzSpeed;
    
    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        Reset();
    }

    public void Reset()
    {
        myColor = Random.ColorHSV(0f,1f,1f,1f,0.5f,1f);
        myColor.a = 0.5f;
        renderer.color = myColor;

        float startingX = Random.Range(-15f, 15f);
        float startingY = Random.Range(-18f, -8f);

        transform.position = new Vector2(startingX, startingY);

        float scale = Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector2(scale, scale);

        vertSpeed = Random.Range(0.05f, 0.15f);
        horzSpeed = Random.Range(-0.15f, 0.15f);
    }

    void FixedUpdate()
    {
        float horzAdjust = Random.Range(-0.01f, 0.01f);
        horzSpeed += horzAdjust;
        horzSpeed = Mathf.Clamp(horzSpeed, -0.15f, 0.15f);

        float vertAdjust = Random.Range(-0.01f, 0.01f);
        vertSpeed += vertAdjust;
        vertSpeed = Mathf.Clamp(vertSpeed, -0.01f, 0.15f);

        transform.position = transform.position + Vector3.up * vertSpeed;
        transform.position = transform.position + Vector3.right * horzSpeed;

        if (transform.position.y > 10f || transform.position.y < -20f)
        {
            Reset();
        }

        if (transform.position.x < -15f || transform.position.x > 15f)
        {
            Reset();
        }
    }
}
