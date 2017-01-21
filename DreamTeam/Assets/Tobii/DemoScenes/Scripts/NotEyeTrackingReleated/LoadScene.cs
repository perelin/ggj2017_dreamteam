using UnityEngine;

public class LoadScene : MonoBehaviour
{
    /// <summary>
    /// Loads a scene by the name of the scene
    /// </summary>
    /// <remarks>
    /// Referenced in the Inspector by OnClick() event of button that loads the
    /// next scene.
    /// </remarks>
    public void Load(string sceneName)
    {
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#else
		Application.LoadLevel(sceneName);
#endif
    }
}
