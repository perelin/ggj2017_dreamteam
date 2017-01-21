using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeMoodAction : MonoBehaviour {

    public SpriteRenderer MoodSprite;
    public Color Color;



    void PerformAction()
    {
        MoodSprite.color = Color;
    }
}
