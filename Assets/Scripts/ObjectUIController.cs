using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LookAtCamera))]
public class ObjectUIController : MonoBehaviour
{
    [Header("Preview UI Components:")]
    public Text nameText;
    public Text healthText;
    public Text statusText;

    private ObjectController parentObj;

    // Fix any potential missing camera issues
    private void Awake()
    {
        if(GetComponent<Canvas>().worldCamera == null)
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    // Link the object values to the UI
    void Start()
    {

        parentObj = transform.parent.GetComponent<ObjectController>();

        // Assign Preview UI Values
        nameText.text = parentObj.objectScript.name + " (#" + parentObj.objectScript.ID + ")";
        healthText.text = parentObj.objectScript.health.ToString();
        statusText.text = parentObj.objectScript.status;

        if (parentObj.objectScript.type == ObjectType.Worker)
            statusText.text += " (" + parentObj.GetComponent<Worker>().task + ")";
    }

}
