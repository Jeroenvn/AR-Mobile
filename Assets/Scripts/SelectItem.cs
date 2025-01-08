using UnityEngine;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private PlaceItem placeItem;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private AudioClip placeSound;

    public void OnSelect()
    {
        if (itemPrefab == null)
        {
            throw new System.Exception("No Item Prefab Selected");
        }

        if (placeItem == null)
        {
            throw new System.Exception("No PlaceItem Selected");
        }

        placeItem.ItemPrefab = itemPrefab;
        placeItem.PlaceSound = placeSound;
    }
}
