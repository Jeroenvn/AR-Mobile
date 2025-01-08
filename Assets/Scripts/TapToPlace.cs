using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    private TouchControlsClass playerInput;
    
    private void Start()
    {
        playerInput = new TouchControlsClass();
        playerInput.Enable();
        SubscribeToPlayerInput();
    }

    private void SubscribeToPlayerInput()
    {
        if (playerInput == null)
        {
            throw new System.Exception("No player input");
        }
        playerInput.FindAction("Touch").performed += OnTouch;
    }

    private void OnTouch(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Debug.Log(touchPosition);

        List<ARRaycastHit> hit = new();
        TrackableType trackableType = TrackableType.PlaneWithinPolygon;
        if (raycastManager.Raycast(touchPosition, hit, trackableType))
        {
            Debug.Log("Hit plane");
        }
    }
}
