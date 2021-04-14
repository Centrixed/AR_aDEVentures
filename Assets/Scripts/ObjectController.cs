using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [Header("Required Component:")]
    public Object objectScript;
    public GameObject objectUI;

    private bool highlight;
    private Color origColor;

    // Start is called before the first frame update
    void Start()
    {
        highlight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (highlight)
        {
            try
            {
                transform.GetComponent<MeshRenderer>().material.color = Color.Lerp(origColor, Color.blue, Mathf.Sin(Time.time * 2));
            }
            catch
            {
                return;
            }
        }
    }

    /// <summary>
    /// Called when the object is touched in observation mode
    /// </summary>
    public void Highlight()
    {
        Debug.Log("Object Highlighted");
        highlight = true;
        EnablePreviewUI();

        try
        {
            origColor = transform.GetComponent<MeshRenderer>().material.color;
        }
        catch
        {
            return;
        }
    }

    /// <summary>
    /// Selects the object and opens the full menu
    /// </summary>
    public void Select()
    {
        Debug.Log("Object Selected");
        return;
    }

    /// <summary>
    /// Deselects & De-highlights the object
    /// </summary>
    public void Deselect()
    {
        Debug.Log("Object Deselected");
        highlight = false;
        DisablePreviewUI();

        try
        {
            transform.GetComponent<MeshRenderer>().material.color = origColor;
        }
        catch
        {
            return;
        }

    }

    /// <summary>
    /// Enables the mini UI above an object
    /// </summary>
    private void EnablePreviewUI()
    {
        if (objectScript.type != ObjectType.Worker)
            return;

        objectUI.SetActive(true);
    }

    /// <summary>
    /// Enables the mini UI above an object
    /// </summary>
    private void DisablePreviewUI()
    {
        if (objectScript.type != ObjectType.Worker)
            return;

        objectUI.SetActive(false);
    }
}
