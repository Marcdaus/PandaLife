using System;
using UnityEngine;

public class movement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float playerSpeed = 7.0f;
    [SerializeField] private Animator anim;

    void Start()
    {
      controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.keepAnimatorStateOnDisable = false;
        }
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v).normalized;
        Debug.Log($"Input: {move}");

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
            if (anim != null) anim.SetFloat("Walking", playerSpeed);
        }
        else
        {
            if(anim!=null) anim.SetFloat("Walking", 0.0f);
        }

            controller.SimpleMove(moveDir * playerSpeed);
    }
}
