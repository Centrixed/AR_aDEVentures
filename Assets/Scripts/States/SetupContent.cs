using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class SetupContent : State
{
    private bool touchedContent, pinchScaling;
    private float lastPinch;
    private static List<ARRaycastHit> touchHits = new List<ARRaycastHit>();
    private SubState currentSubState;

    // List of Sub-States for the setup process
    private enum SubState
    {
        begin,
        placement,
        adjustment
    }

    // Constructor to grab touch & content controllers
    public SetupContent(ContentController contentController) : base(contentController)
    {
    }

    // At the beginning of the state, setup visuals and booleans
    public override IEnumerator Start()
    {
        currentSubState = SubState.begin;
        touchedContent = false;
        pinchScaling = false;
        lastPinch = 0;

        if (contentController.content != null)
            contentController.content.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Called by the ContentController whenever a user is touching with one finger (or more than two)
    /// </summary>
    /// <param name="fingerCount"></param>
    public override void Touch()
    {
        Touch touch = Input.GetTouch(0);

        // If the player is clicking a UI element
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        // If the substate is object placement
        if (currentSubState == SubState.begin || currentSubState == SubState.placement) 
        {
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                // Shoot a raycast and attempt to find a plane to place object upon
                if (contentController.m_RaycastManager.Raycast(touch.position, touchHits, TrackableType.PlaneWithinPolygon))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = touchHits[0].pose;

                    // This does not move the content; instead, it moves and orients the ARSessionOrigin
                    // such that the content appears to be at the raycast hit position.
                    contentController.m_SessionOrigin.MakeContentAppearAt(contentController.content, hitPose.position, contentController.rotation);

                    // Enable the content visuals once tapped
                    if (contentController.content != null)
                        contentController.content.gameObject.SetActive(true);

                    // If this is the first placement, show the content and move to next substate
                    if (currentSubState == SubState.begin)
                    {
                        contentController.contentLockButton.transform.parent.gameObject.SetActive(true);
                        currentSubState = SubState.placement;
                    }
                }
            }
        }
        // If the substate is object scale/rotation
        else if(currentSubState == SubState.adjustment)
        {
            // If touched content, allow for rotation/scaling through a boolean
            if(touch.phase == TouchPhase.Began && !touchedContent)
            {
                touchedContent = CheckForContentTouch(touch);
            }
            // If content is placed and finger is moving, rotate on Y-Axis
            else if(touch.phase == TouchPhase.Moved && touchedContent)
            {
                var objRot = contentController.rotation.eulerAngles;
                Quaternion finalRotation = Quaternion.Euler(objRot.x, objRot.y + (-touch.deltaPosition.x * contentController.rotateSpeedModifier), objRot.z);

                contentController.m_SessionOrigin.MakeContentAppearAt(contentController.content, contentController.content.transform.position, finalRotation);
                contentController.rotation = finalRotation;
            }
        }

        return;
    }

    /// <summary>
    /// Called by content controller when exactly two fingers are touching the screen
    /// </summary>
    /// <param name="deltaValue"></param>
    public override void Pinch()
    {
        Touch finger1 = Input.GetTouch(0);
        Touch finger2 = Input.GetTouch(1);
        float currentPinch = Vector3.Distance(finger1.position, finger2.position) / 2;

        if (currentSubState == SubState.adjustment)
        {
            // Set the initial distance between both fingies
            if (finger1.phase == TouchPhase.Began || finger2.phase == TouchPhase.Began)
            {
                touchedContent = CheckForContentTouch(finger1) || CheckForContentTouch(finger2);
                lastPinch = Vector3.Distance(finger1.position, finger2.position) / 2;
                pinchScaling = true;
            }
            // Scale the object as the fingies move
            else if (finger1.phase == TouchPhase.Moved || finger2.phase == TouchPhase.Moved)
            {
                if (!touchedContent)
                    return;

                if (!pinchScaling)
                {
                    lastPinch = Vector3.Distance(finger1.position, finger2.position) / 2;
                    pinchScaling = true;
                    return;
                }

                float deltaPinch = (currentPinch - lastPinch) * -contentController.scaleSpeedModifier;
                float currentScale = contentController.m_SessionOrigin.transform.localScale.x;
                float adjustedScale = currentScale + deltaPinch;

                contentController.m_SessionOrigin.transform.localScale = Vector3.one * adjustedScale;
                lastPinch = currentPinch;
            }

        }

        return;
    }

    /// <summary>
    /// Called by ContentController when there are no fingies touching the screen
    /// </summary>
    public override void NoTouch()
    {
        if (touchedContent)
            touchedContent = false;

        if (pinchScaling)
            pinchScaling = false;

        return;
    }

    /// <summary>
    /// Raycasts from where the user touched the screen. If it hits content, return true
    /// </summary>
    private bool CheckForContentTouch(Touch t)
    {
        // Check if the user is touching a content item with their finger(s)
        Ray raycast = Camera.main.ScreenPointToRay(t.position);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("ContentContainer"))
            {
                contentController.setupCompleteButton.gameObject.SetActive(true);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Called when the content lock button has been pressed
    /// </summary>
    public override void ContentLock_Click()
    {
        currentSubState = SubState.adjustment;
        contentController.contentLockButton.transform.parent.gameObject.SetActive(false);
        return;
    }
}
