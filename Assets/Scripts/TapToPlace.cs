using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
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
            Instantiate(prefab, hit[0].pose.position, Quaternion.identity);
            playerInput.FindAction("Touch").performed -= OnTouch;
            planeManager.SetTrackablesActive(false);
            planeManager.enabled = false;
        }
    }
}
