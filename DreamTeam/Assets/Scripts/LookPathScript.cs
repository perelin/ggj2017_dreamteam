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

    private float currentProgress = 0;

    public float speed = 4f;

    // Use this for initialization
    void Start ()
	{
        Assert.IsNotNull(gazeComponent);
        gazeComponent = InidiratorGameObject.GetComponent<GazeAware>();
	}
	
	// Update is called once per frame
	void Update () {

        /*GazePoint gazePoint = EyeTracking.GetGazePoint();
        if (gazePoint.IsValid)
        {
            print("Gaze point on Screen (X,Y): " + gazePoint.Screen.x + ", " + gazePoint.Screen.y);
            var point = new Vector3(gazePoint.Screen.x, gazePoint.Screen.y);
            var objectPos = Camera.main.ScreenToWorldPoint(point);

            var oldZ = InidiratorGameObject.gameObject.transform.position.z;

            InidiratorGameObject.gameObject.transform.position = new Vector3(objectPos.x, objectPos.y, oldZ);

        }*/

        GameObject focusedObject = EyeTracking.GetFocusedObject();
        if (null != focusedObject)
        {
            print("The focused game object is: " + focusedObject.name + " (ID: " + focusedObject.GetInstanceID() + ")");
        }



        if (gazeComponent.HasGazeFocus)
	    {
            Debug.Log("Has Focus");
	        currentProgress += speed * Time.deltaTime;
	    }
        
        /*

	    EyeTracking.GetFocusedObject();

        Vector3 nextPos = FollowCurve.GetPointAt(currentProgress);
        InidiratorGameObject.gameObject.transform.position = nextPos;

        */
    }
}
