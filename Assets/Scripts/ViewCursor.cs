using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ViewCursor : MonoBehaviour
{
    // Public Variables:
    [Header("Accessible Data:")]
    public Vector3 cursorPosition;

    [Header("Settings:")]
    public bool EnablePlaneDetection;
    public bool EnableObjectDetection;

    [Header("Required:")]
    public GameObject cursorPrefab;

    // Private Variables:
    private ARRaycastManager rayManager;
    private GameObject cursorVisual;
    private Vector2 screenCenter;
    private Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        // Find the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        cursorVisual = Instantiate(cursorPrefab, transform.position, Quaternion.identity);

        // Hide the visual
        cursorVisual.SetActive(false);

        // Calculate the center of the screen
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // Set the default cursor position
        cursorPosition = Vector3.zero;

        // Cursor Debug Info
        debugText = GameObject.FindGameObjectWithTag("DebugVisual").GetComponent<Text>();
    }

    private void OnDestroy()
    {
        if (cursorVisual != null)
            Destroy(cursorVisual);
    }

    // Update is called once per frame
    void Update()
    {
        if (EnablePlaneDetection)
            DetectPlanes();

        if (cursorPosition != Vector3.zero)
            debugText.text = cursorPosition.ToString();
    }

    /// <summary>
    /// Attempts to place the cursor on planes if found
    /// </summary>
    private void DetectPlanes()
    {
        // Shoot a raycast from center of screen that looks for planes
        List<ARRaycastHit> planeHits = new List<ARRaycastHit>();
        rayManager.Raycast(screenCenter, planeHits, TrackableType.Planes);

        // Detect plane hits
        if (planeHits.Count > 0)
        {
            // Move visual to ray hit and set visual to true
            cursorVisual.transform.position = planeHits[0].pose.position;
            cursorVisual.transform.rotation = planeHits[0].pose.rotation;

            // Log the cursorPosition for global availability
            cursorPosition = cursorVisual.transform.position;

            // Set visual to active
            if (!cursorVisual.activeInHierarchy)
                cursorVisual.SetActive(true);
        }
        else
        {
            // Hide the visual if no plane detected
            if (cursorVisual.activeInHierarchy)
                cursorVisual.SetActive(false);

            // Reset the visual to the parent object's location and also reset the global cursor position
            cursorVisual.transform.position = Vector3.zero;
            cursorPosition = Vector3.zero;
        }
    }
}
