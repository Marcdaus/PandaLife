using UnityEngine;

public class movement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float playerSpeed = 7.0f;

    void Start()
    {
      controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);

        // Obtener la dirección de la cámara (solo eje Y)
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Convertir movimiento relativo a cámara
        Vector3 moveDir = camForward * v + camRight * h;

        if (moveDir != Vector3.zero)
        {
            transform.forward = moveDir;
        }

        controller.SimpleMove(moveDir * playerSpeed);
    }
}
