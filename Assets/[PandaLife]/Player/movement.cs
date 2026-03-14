using UnityEngine;

public class movement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float playerSpeed = 7.0f;
    [SerializeField] private bool useCameraRef = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        if (useCameraRef)
        {
            move = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * move;
        }

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        controller.SimpleMove(move * playerSpeed);

    }
}
