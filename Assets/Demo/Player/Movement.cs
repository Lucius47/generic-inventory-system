using UnityEngine;

namespace Demo
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        private void Update()
        {
            Vector2 movement = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (movement != Vector2.zero)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.y));
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
            }
        }
    }
}