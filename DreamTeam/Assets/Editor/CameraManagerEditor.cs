using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        CameraManager myTarget = (CameraManager)target;

        myTarget.Zoom = EditorGUILayout.FloatField("Zoom", myTarget.Zoom);
        if (myTarget.Zoom < 3)
        {
            myTarget.Zoom = 3;
        }
        if (myTarget.Zoom > 50)
        {
            myTarget.Zoom = 50;
        }
       // DrawDefaultInspector();
    }
}