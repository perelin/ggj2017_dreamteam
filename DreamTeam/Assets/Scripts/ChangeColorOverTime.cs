using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOverTime : MonoBehaviour {

    public Color TargetColor;
    public float LerpTime;
    private float timeElapsed;
    private SpriteRenderer _spriteRenderer;
    private Color initialColor;

    // Use this for initialization
    void Start()
    {
        timeElapsed = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = _spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > LerpTime)
        {
            // finish 
            _spriteRenderer.color = TargetColor;
            Destroy(this);
            return;
        }
        _spriteRenderer.color = Color.Lerp(initialColor, TargetColor, timeElapsed / LerpTime);

    }
}
