using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceItem : MonoBehaviour
{
    public GameObject ItemPrefab;
    public AudioClip PlaceSound;

    [SerializeField] private AudioClip RemoveSound;
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GraphicRaycaster graphicRaycaster;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private TouchControlsClass playerInput;
    private EventSystem eventSystem;
    private AudioSource audioSource;

    private GameObject itemInstance = null;

    private void Start()
    {
        SubscribeMethods();
        SetupManagers();
        eventSystem = GetComponent<EventSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void SubscribeMethods()
    {
        if (playerInput == null)
        {
            playerInput = new TouchControlsClass();
            playerInput.Enable();
        }
        playerInput.FindAction("Touch").performed += OnTouch;
    }

    private void UnsubscribeMethods()
    {
        playerInput.FindAction("Touch").performed -= OnTouch;
    }

    private void SetupManagers()
    {
        if (xrOrigin == null)
        {
            throw new Exception("No XROrigin chosen");
        }
        raycastManager = xrOrigin.GetComponent<ARRaycastManager>();
        planeManager = xrOrigin.GetComponent<ARPlaneManager>();
    }

    private void OnTouch(InputAction.CallbackContext context)
    {
        Vector2 touchScreenPosition = context.action.ReadValue<Vector2>();

        if (TouchedUI(touchScreenPosition))
        {
            return;
        }

        if (itemInstance == null)
        {
            TryPlaceItem(touchScreenPosition);
            return;
        }

        RemovePlacedItem();
    }

    private bool TouchedUI(Vector2 touchScreenPosition)
    {
        List<RaycastResult> raycastResults = new();
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = touchScreenPosition;

        graphicRaycaster.Raycast(pointerEventData, raycastResults);
        bool touchedScreen = raycastResults.Count > 0;

        return touchedScreen;
    }

    private void TryPlaceItem(Vector2 touchScreenPosition)
    {
        if (ItemPrefab == null)
        {
            throw new Exception("ItemPrefab is not assigned");
        }

        List<ARRaycastHit> hit = new();
        TrackableType trackableType = TrackableType.PlaneWithinPolygon;

        if (raycastManager.Raycast(touchScreenPosition, hit, trackableType))
        {
            itemInstance = Instantiate(ItemPrefab, hit[0].pose.position, Quaternion.identity);
            audioSource.PlayOneShot(PlaceSound);
            SetPlanesActive(false);
        }
    }

    private void RemovePlacedItem()
    {
        Destroy(itemInstance);
        itemInstance = null;
        SetPlanesActive(true);
    }

    private void SetPlanesActive(bool active)
    {
        planeManager.enabled = active;
        planeManager.SetTrackablesActive(active);
    }
}
