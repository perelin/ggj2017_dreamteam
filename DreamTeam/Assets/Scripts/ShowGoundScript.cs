using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGoundScript : MonoBehaviour {

    public Color darknessColor;
    //public GameObject root;
    public string layerName = "Ground";

	// Use this for initialization
	void Start () {
        var goList = ShowGoundScript.FindGameObjectsWithLayer(LayerMask.NameToLayer(layerName));

        foreach (var go in goList)
        {
            // for every element on the layer 'ground' put a copy on the 'darkness' layer
            var goSR = go.GetComponent<SpriteRenderer>();

            if (goSR == null)
            {
                Debug.Log("ShowGoundScript:: no SpriteRenderer for: " + go.name);
                return;
            }

            var newGo = new GameObject();
            newGo.transform.SetParent(go.transform, false);
            newGo.name = go.name + "-" + layerName;
            newGo.transform.Translate(new Vector3(0, 0, -5));
            newGo.layer = LayerMask.NameToLayer("Darkness");

            var sr = newGo.AddComponent<SpriteRenderer>();
            sr.sprite = goSR.sprite;
            sr.color = darknessColor;
        }
    }
    
    private static List<GameObject> GetObjectsInLayer(GameObject root, int layer)
    {
        var ret = new List<GameObject>();
        foreach (Transform t in root.transform.GetComponentsInChildren(typeof(GameObject), true))
        {
            if (t.gameObject.layer == layer)
            {
                ret.Add(t.gameObject);
            }
        }
        return ret;
    }

    private static List<GameObject> FindGameObjectsWithLayer(int layer) {
         var goArray = FindObjectsOfType<GameObject>();
         var goList = new List<GameObject>();
        foreach (var go in goArray)
        {
             if (go.layer == layer) {
                 goList.Add(go);
             }
         }
         if (goList.Count == 0) {
             return null;
         }
         return goList;
     }
}
