using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialDiscoveryComponent : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    private float timeElapsed;
    public float LerpTime;
    public GameObject Tutorial;
    public float TutorialTime;

    void Start()
    {
        timeElapsed = 0;
     
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > TutorialTime)
        {
            Tutorial.SetActive(true);
        }

        if (timeElapsed > LerpTime)
        {
            // finish 
            SpriteRenderer.color = Color.white;
            Destroy(this);

            return;
        }
        SpriteRenderer.color = new Color(1,1,1, timeElapsed / LerpTime);

    }
}
