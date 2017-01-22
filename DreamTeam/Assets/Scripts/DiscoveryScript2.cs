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
    RaycastHit2D hit;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var trackPos = TrackingStuff.getTrackingPos();

        ray = Camera.main.ScreenPointToRay(TrackingStuff.getTrackingPos());
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        // if collision: increment timer
        if (hit.collider != null && hit.collider == this.gameObject.GetComponent<Collider2D>())
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
