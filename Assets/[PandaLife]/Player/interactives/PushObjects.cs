using UnityEngine;

public class PushObjects : MonoBehaviour
{
    public float pushpower = 2.0f; // Fuerza con la que empuja el objeto

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

        // Dirección del empuje
        Vector3 pushdir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Aplicar fuerza al objeto
        body.linearVelocity = pushdir * pushpower;
    }
}