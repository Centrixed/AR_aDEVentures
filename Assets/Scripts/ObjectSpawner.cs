using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    private ViewIndicator viewIndicator;

    // Start is called before the first frame update
    void Start()
    {
        viewIndicator = FindObjectOfType<ViewIndicator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (viewIndicator.EnableTargetVisual)
        {
            // Check for touch inputs
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                GameObject obj = Instantiate(objectPrefab, viewIndicator.transform.position, viewIndicator.transform.rotation);
            }
        }
    }
}
