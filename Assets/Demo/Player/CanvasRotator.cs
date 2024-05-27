using UnityEngine;

namespace Demo
{
    /// <summary>
    /// Rotates the canvas to face the camera.
    /// </summary>
    public class CanvasRotator : MonoBehaviour
    {
        private Transform cameraTransform;

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        }
    }
}
