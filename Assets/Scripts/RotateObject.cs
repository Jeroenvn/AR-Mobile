using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private string objectTag;
    [SerializeField] private int rotationDegrees;

    private GameObject objectToRotate;
    private Canvas infoCanvas;

    private void Update()
    {
        LookForObject();
    }

    public void RotateLeft()
    {
        objectToRotate.transform.Rotate(Vector3.up * rotationDegrees);
    }

    public void RotateRight()
    {
        objectToRotate.transform.Rotate(Vector3.up * -rotationDegrees);
    }

    public void ToggleInfo()
    {
        infoCanvas.enabled = !infoCanvas.enabled;
    }

    public void LookForObject()
    {
        if (objectToRotate != null)
        {
            return;
        }

        objectToRotate = GameObject.FindGameObjectWithTag(objectTag);

        if (objectToRotate != null)
        {
            infoCanvas = objectToRotate.GetComponentInChildren<Canvas>();
        }
    }
}
