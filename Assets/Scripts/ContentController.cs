using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

/// <summary>
/// Moves the ARSessionOrigin in such a way that it makes the given content appear to be
/// at a given location acquired via a raycast.
/// </summary>
[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]

public class ContentController : StateMachine
{
    [Header("Modifiers:")]
    public float rotateSpeedModifier;
    public float scaleSpeedModifier;

    [Header("Required Components:")]
    public Button contentLockButton;
    public Button setupCompleteButton;
    public Transform content;

    [Header("Data Visuals (DNE):")]
    public Quaternion rotation;
    public ARSessionOrigin m_SessionOrigin;
    public ARRaycastManager m_RaycastManager;

    private bool touching;


    // Start is called before the first frame update
    void Start()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_RaycastManager = GetComponent<ARRaycastManager>();

        SetState(new SetupContent(this));
        touching = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    /// <summary>
    /// Checks for touches, drags, or pinches, then informs the state
    /// </summary>
    void CheckInputs()
    {   
        // If no touches, call the NoTouch function and switch the bool to false
        if (Input.touchCount == 0 && touching) {
            State.NoTouch();
            touching = false;
            return;
        }

        // If touches equal 1 OR greater than 2 (aka not pinching)
        if(Input.touchCount == 1 || Input.touchCount > 2)
        {
            State.Touch();
            touching = true;
        }
        else if(Input.touchCount == 2)
        {
            State.Pinch();
            touching = true;
        }
    }

    /// <summary>
    /// Calls a state function when the content lock button has been pressed
    /// </summary>
    public void ContentLock_OnClick()
    {
        State.ContentLock_Click();
    }

    /// <summary>
    /// Sets the state to Observation when the ready button has been pressed
    /// </summary>
    public void SetupComplete_OnClick()
    {
        Debug.Log("Switching to Observation State");
        SetState(new Observation(this));
        content.GetComponent<BoxCollider>().enabled = false;
        setupCompleteButton.gameObject.SetActive(false);
    }
}
