using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class Observation : State
{
    private static List<ARRaycastHit> touchHits = new List<ARRaycastHit>();
    private SubState currentSubState;
    private ObjectController highlightedObject;

    // List of Sub-States for the setup process
    private enum SubState
    {
        Spectate,
        Highlight
    }

    // Constructor to grab touch & content controllers
    public Observation(ContentController contentController) : base(contentController)
    {
    }

    // At the beginning of the state, setup visuals and booleans
    public override IEnumerator Start()
    {
        currentSubState = SubState.Spectate;

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

        // If touched content, allow for rotation/scaling through a boolean
        if (touch.phase == TouchPhase.Began)
        {
            // Check if the user is touching a content item with their finger(s)
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("ContentObject"))
                {
                    // Remove previously highlighted object
                    if (highlightedObject != null)
                        highlightedObject.Deselect();

                    // Highlight the newly touched object
                    highlightedObject = raycastHit.transform.GetComponent<ObjectController>();
                    highlightedObject.Highlight();
                }
                
                if (raycastHit.collider.CompareTag("ObjectUIButton"))
                {
                    Debug.Log("Object UI Button Pressed");
                }
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

        return;
    }

    /// <summary>
    /// Called by ContentController when there are no fingies touching the screen
    /// </summary>
    public override void NoTouch()
    {

        return;
    }
}
