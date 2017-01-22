using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableScript : MonoBehaviour
{

    public GameObject[] GameObjects;
    public Behaviour[] Behaviours;

    void PerformAction()
    {
        foreach (var go in GameObjects)
        {
            go.SetActive((true));
        }

        foreach (var bhv in Behaviours)
        {
            bhv.enabled = true;
        }
    }
}
