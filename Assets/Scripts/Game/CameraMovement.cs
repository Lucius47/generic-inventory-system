using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed = 0.1f;


    private void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void LateUpdate()
    {
        // smoothly follow the player
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, speed);
    }
}
