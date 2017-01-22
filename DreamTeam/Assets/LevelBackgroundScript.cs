using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBackgroundScript : MonoBehaviour {

    public float scrollSpeed = 1;
    public float tileSizeZ = 100;

    public float wobbleSpeed = 0.5f;
    public float wobbleAplitude = 1;

    private Vector3 startPosition;

    void Start()
    {
        Vector2 scale = this.gameObject.transform.localScale;
        int width = (int)(scale.x / scale.y);

        this.gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(width, 1);

        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        float newSin = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAplitude;
        transform.position = startPosition + Vector3.left * newPosition + Vector3.up * newSin;
    }
}
