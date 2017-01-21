using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        CameraManager myTarget = (CameraManager)target;

        myTarget.Size = EditorGUILayout.FloatField("Zoom", myTarget.Size);
        if (myTarget.Size < 3)
        {
            myTarget.Size = 3;
        }
        if (myTarget.Size > 50)
        {
            myTarget.Size = 50;
        }
    }
}