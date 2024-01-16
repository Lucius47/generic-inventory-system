using UnityEngine;

public class CanvasRotator : MonoBehaviour
{
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // rotate the canvas to face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
