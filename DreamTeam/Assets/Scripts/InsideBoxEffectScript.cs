using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideBoxEffectScript : MonoBehaviour {
    private bool playEffect = false;
    public MonoBehaviour EffectUpdatedWhenInBox;

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            playEffect = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            playEffect = false;
        }
    }

    void Update()
    {
        if (playEffect)
        {
            if (EffectUpdatedWhenInBox != null)
            {
                EffectUpdatedWhenInBox.SendMessage("InsideBoxUpdate");
            }
        }
    }
}
