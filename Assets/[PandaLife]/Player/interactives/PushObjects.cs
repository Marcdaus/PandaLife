using UnityEngine;

public class PushObjects : MonoBehaviour
{
    public float pushPower = 2.0f; // Fuerza con la que empuja el objeto

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Comprobamos si el objeto tiene Rigidbody
        Rigidbody body = hit.collider.attachedRigidbody;

        // Si no tiene Rigidbody o es kinematic, no hacemos nada
        if (body == null || body.isKinematic)
        {
            return;
        }

        // Evita empujar objetos hacia abajo
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        // Direcci�n del empuje
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Aplicar fuerza al objeto
        body.linearVelocity = pushDir * pushPower;
    }
}
