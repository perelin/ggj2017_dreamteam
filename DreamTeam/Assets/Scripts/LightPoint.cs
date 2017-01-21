using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint : MonoBehaviour
{

    public float DecreasePerSecond = 0.5f;
    public float MinimalSizeBeforeDie = 0.001f;
    private SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Start ()
	{
	    _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        float delta = DecreasePerSecond * Time.deltaTime;
        var color = _spriteRenderer.color;
        color.a -= delta;
        _spriteRenderer.color = color;

	    if (color.a < MinimalSizeBeforeDie)
	    {
	        Destroy(gameObject);
	    }
	    /* transform.localScale -= new Vector3(delta, delta, delta);
   
         if (transform.localScale.x < 0.001)
         {
             Destroy(gameObject);
         }*/
	}
}
