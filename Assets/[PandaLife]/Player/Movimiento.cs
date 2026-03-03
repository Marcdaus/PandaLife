using UnityEngine;

public class movimiento : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float playerSpeed = 2.0f;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
  

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        controller.SimpleMove(move * playerSpeed);

    }
}
