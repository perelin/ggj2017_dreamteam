using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tobii.EyeTracking;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;



public class LookPathScript : MonoBehaviour
{
    private GazeAware gazeComponent;
    public GameObject InidiratorGameObject;

    public BezierCurve FollowCurve;

    private float currentProgress = 0f;

    public float speed = 4f;

    private float progressPerTime;

    public bool useGazeFocus = false;

    // Use this for initialization
    void Start ()
    {
        progressPerTime = speed / FollowCurve.length;
        gazeComponent = InidiratorGameObject.GetComponent<GazeAware>();
        Assert.IsNotNull(gazeComponent);
    }
	
	// Update is called once per frame
	void Update () {
	    if (useGazeFocus)
	    {
	        if (gazeComponent.HasGazeFocus)
	        {
	            currentProgress = Mathf.Min(currentProgress + speed * Time.deltaTime, 1f);
	        }
	    }
	    else
	    {
	        GazePoint gazePoint = EyeTracking.GetGazePoint();
	        if (gazePoint.IsValid)
	        {
                var ray = Camera.main.ScreenPointToRay(gazePoint.Screen);
                var hit = Physics2D.Raycast(ray.origin, ray.direction);

                // if collision: increment timer
	            if (hit.collider != null && hit.collider.gameObject == this.InidiratorGameObject)
	            {
                    Debug.Log("hit");
                    currentProgress = Mathf.Min(currentProgress + speed * Time.deltaTime, 1f);
                }
	        }
	    }

	    Vector3 nextPos = FollowCurve.GetPointAt(currentProgress);
        InidiratorGameObject.gameObject.transform.position = nextPos;
    }
}
