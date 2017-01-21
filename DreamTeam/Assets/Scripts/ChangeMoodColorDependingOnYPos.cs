using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMoodColorDependingOnYPos : MonoBehaviour
{
    public SpriteRenderer MoodColor;

    public Transform Player;

    public float LowY;
    public Color LowColor;
    public float HighY;
    public Color HighColor;

    void InsideBoxUpdate()
    {
        MoodColor.color = calculateColor(Player.transform.position.y);
    }

    private Color calculateColor(float y)
    {
        if (y < LowY)
        {
            return LowColor;
        }
        if (y > HighY)
        {
            return HighColor;
        }
        return Color.Lerp(LowColor, HighColor, (y - LowY) / (HighY - LowY));

    }
}
