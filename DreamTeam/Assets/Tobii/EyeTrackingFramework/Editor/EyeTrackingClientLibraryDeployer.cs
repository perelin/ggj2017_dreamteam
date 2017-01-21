//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------
 
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
 
/// <summary>
/// Editor class for deployment of the Tobii EyeX Client library and .NET binding
/// to where they need to be.
/// </summary>
[InitializeOnLoad]
public class EyeTrackingClientLibraryDeployer
{
    private const string ClientLibraryFileName = "Tobii.EyeX.Client.dll";
    private const string Source32BitDirectory  = "Assets/Tobii/Plugins/x86";
    private const string Source64BitDirectory  = "Assets/Tobii/Plugins/x86_64";
 
    /// <summary>
    /// When loading the editor, copy the correct version of the EyeX client
    /// library to the project root folder to be able to run in the editor.
    /// </summary>
    static EyeTrackingClientLibraryDeployer()
    {
        var targetClientLibraryPath = Path.Combine(Directory.GetCurrentDirectory(), ClientLibraryFileName);
        if(!File.Exists(targetClientLibraryPath))
        {
            if(System.IntPtr.Size == 8)
            {
                Debug.Log("Initialize: Copying Tobii Client x64 native library.");
                Copy64BitClientLibrary(targetClientLibraryPath);
            }
            else
            {
                Debug.Log("Initialize: Copying Tobii Client x86 native library.");
                Copy32BitClientLibrary(targetClientLibraryPath);
            }
        }

        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinux ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinux64 ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinuxUniversal ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSXIntel ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSXIntel64 ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSXUniversal)
        {
            Debug.LogWarning("Tobii EyeTracking Framework can only provide eye-gaze data on the Windows platform. Change platform in build settings to make the EyeTracking features work.");
        }
        else if (!(EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows ||
              EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64))
        {
            Debug.LogError("Tobii EyeTracking Framework only supports building for Standalone build targets. Eye-gaze data is only available on the Windows platform. Change platform in build settings to make the EyeTracking features work.");
        }                          
    }
 
    /// <summary>
    /// After a build, copy the correct EyeX client library to the output directory.
    /// </summary>
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        var targetClientLibraryPath = Path.Combine(Path.GetDirectoryName(pathToBuiltProject), ClientLibraryFileName);
        if(!File.Exists(targetClientLibraryPath))
        {
            if (target == BuildTarget.StandaloneWindows)
            {
                Debug.Log("Build: Copying x86 native library.");
                Copy32BitClientLibrary(targetClientLibraryPath);
            }
            else if (target == BuildTarget.StandaloneWindows64)
            {
                Debug.Log("Build: Copying x64 native library.");
                Copy64BitClientLibrary(targetClientLibraryPath);
            }
            else
            {
                Debug.LogWarning("The Tobii EyeTracking Framework for Unity is only compatible with Windows Standalone builds.");
            }
        }
    }
 
    private static void Copy32BitClientLibrary(string targetClientDllPath)
    {
        File.Copy(Path.Combine(Source32BitDirectory, ClientLibraryFileName), targetClientDllPath, true);
    }
 
    private static void Copy64BitClientLibrary(string targetClientDllPath)
    {
        File.Copy(Path.Combine(Source64BitDirectory, ClientLibraryFileName), targetClientDllPath, true);
    }
}