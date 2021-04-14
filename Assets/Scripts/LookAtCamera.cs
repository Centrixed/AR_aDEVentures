using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [Header("Modifiers:")]
    public float distanceScaleModifier;
    public float distanceHeightModifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly rotate the object towards the camera
        if(Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }

        // Update the object's scale based upon camera distance to objects
        float distance = (Camera.main.transform.position - transform.position).magnitude;
        float size = distance * distanceScaleModifier * Camera.main.fieldOfView;
        transform.localScale = Vector3.one * size;

        // Update the object's height based upon camera distance to objects
        distance = (Camera.main.transform.position - transform.position).magnitude;
        float height = distance * distanceHeightModifier * Camera.main.fieldOfView;
        transform.localPosition.Set(transform.localPosition.x, height - 4, transform.localPosition.z);

    }
}
