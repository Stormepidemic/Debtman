using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed;
    public float jumpHeight;
    public float gravityValue = -9.81f;
    private Animator anim;
    public GameObject reference; //The reference frame that rotates to change where 'forward'

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("Movement", 0.0f);
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            anim.SetFloat("Vertical", 0.0f);
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            anim.SetFloat("Movement", 1.0f);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump")){
            if(playerVelocity.y < 0.5 && playerVelocity.y > -0.5)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            anim.SetFloat("Vertical", 1.0f);
        }
        

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}