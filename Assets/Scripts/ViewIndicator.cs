using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ViewIndicator : MonoBehaviour
{
    public GameObject faceVisualPrefab;
    public Text faceTrackingStatus;

    [Header("Settings:")]
    public bool EnableTargetVisual;
    public bool EnableCustomFaceVisual;

    private ARRaycastManager rayManager;
    private ARFaceManager faceManager;
    private GameObject targetVisual;
    private Vector2 screenCenter;
    private ARFace face;
    private GameObject faceVisual;

    // Used for setting up Facial Detection event handlers
    private void OnEnable()
    {
        if (EnableCustomFaceVisual)
        {
            faceManager = FindObjectOfType<ARFaceManager>();
            faceManager.facesChanged += FacesChanged;

            faceVisual = Instantiate(faceVisualPrefab, face.transform.position, face.transform.rotation);
            faceVisual.SetActive(false);
        }
    }

    // Used for disabling the Facial Detection event handlers
    private void OnDisable()
    {
        if(EnableCustomFaceVisual)
            faceManager.facesChanged -= FacesChanged;
    }

    /// <summary>
    /// Called whenever the faces on the camera change. Uses the args to determine what to do
    /// </summary>
    void FacesChanged(ARFacesChangedEventArgs arEventArgs)
    {
        if (EnableCustomFaceVisual)
        {
            if (arEventArgs.updated != null && arEventArgs.updated.Count > 0)
            {
                face = arEventArgs.updated[0];

                faceVisual.transform.position = face.transform.position;
                faceVisual.transform.rotation = face.transform.rotation;

                if (!faceVisual.activeInHierarchy)
                    faceVisual.SetActive(true);

                faceTrackingStatus.color = Color.green;
            }
            else
            {
                if (faceVisual.activeInHierarchy)
                    faceVisual.SetActive(false);

                faceTrackingStatus.color = Color.red;
            }
        }
    }

    private void Start()
    {
        // Find the components
        targetVisual = transform.GetChild(0).gameObject;

        // Hide the visuals
        targetVisual.SetActive(false);

        // Calculate the center of the screen
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (EnableTargetVisual)
            rayManager = FindObjectOfType<ARRaycastManager>();
    
    }

    private void Update()
    {   
        // If the target visual is enabled, calculate it's location and display it
        if (EnableTargetVisual)
        {
            // Shoot a raycast from center of screen that looks for planes
            List<ARRaycastHit> planeHits = new List<ARRaycastHit>();
            rayManager.Raycast(screenCenter, planeHits, TrackableType.Planes);

            // Detect plane hits
            if (planeHits.Count > 0)
            {
                // Move visual to ray hit and set visual to true
                transform.position = planeHits[0].pose.position;
                transform.rotation = planeHits[0].pose.rotation;

                if (!targetVisual.activeInHierarchy)
                    targetVisual.SetActive(true);
            }
            else
            {
                // Hide the visual if no plane detected
                if (targetVisual.activeInHierarchy)
                    targetVisual.SetActive(false);
            }
        }
    }
}
