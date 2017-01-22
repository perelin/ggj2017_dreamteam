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
		Debug.Log (calculateColor (Player.transform.position.y));
        MoodColor.color = calculateColor(Player.transform.position.y);
    }

    private Color calculateColor(float y)
    {
        if (y < LowY)
        {
			Debug.Log ("low");
            return LowColor;
        }
        if (y > HighY)
        {
			Debug.Log ("high");
            return HighColor;
        }

		Debug.Log ("calc");
        return Color.Lerp(LowColor, HighColor, (y - LowY) / (HighY - LowY));

    }
}
