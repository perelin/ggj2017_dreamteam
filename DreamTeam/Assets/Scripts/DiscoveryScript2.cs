using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]


public class DiscoveryScript2 : MonoBehaviour
{

    public MonoBehaviour EnterScript;
    public MonoBehaviour ExitScript;

    private bool inside = false;

    Ray ray;
    RaycastHit2D[] hits;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var trackPos = TrackingStuff.getTrackingPos();

        ray = Camera.main.ScreenPointToRay(TrackingStuff.getTrackingPos());
        hits = Physics2D.RaycastAll(ray.origin, ray.direction);

        bool hasHit = false;

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                hasHit = true;
                break;
            }
        }

        if (hasHit)
        {
            if (!inside)
            {
                EnterScript.SendMessage("PerformEnter");
                inside = true;
            }
        }
        else
        {
            if (inside)
            {
                ExitScript.SendMessage("PerformExit");
                inside = false;
            }
        }
    }
}
