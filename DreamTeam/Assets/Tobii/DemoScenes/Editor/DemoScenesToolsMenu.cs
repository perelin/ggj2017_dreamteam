//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using UnityEditor;

public class DemoScenesToolsMenu
{
    [MenuItem("Tools/Add Tobii SDK Demo Scenes to Build")]
    static void AddDemoScenesToBuild()
    {
        var scenes = EditorBuildSettings.scenes;
        ArrayUtility.Add(ref scenes, new EditorBuildSettingsScene("Assets/Tobii/DemoScenes/01_EyeTrackingData.unity", true));
        ArrayUtility.Add(ref scenes, new EditorBuildSettingsScene("Assets/Tobii/DemoScenes/02_SimpleGazeSelection.unity", true));
        ArrayUtility.Add(ref scenes, new EditorBuildSettingsScene("Assets/Tobii/DemoScenes/03_EyeTrackingStatusAndConfiguration.unity", true));
        EditorBuildSettings.scenes = scenes;
    }
}