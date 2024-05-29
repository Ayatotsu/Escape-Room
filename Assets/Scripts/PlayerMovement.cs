using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController charController;
    public Animator animator;
    public float speed = 2f;
    public float gravity = 9.81f;
    public float jumpHeight = 4f;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    public bool walking;
    public bool running;
    public bool jump;

    Vector3 velocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        
    }

    void MoveCharacter() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");  //x axis
        float z = Input.GetAxis("Vertical");    //y axis

        if (z < 0 || z > 0 || x < 0 || x > 0)
        {
            walking = true;
        }
        else 
        {
            walking = false;
        }
        animator.SetFloat("Movement", z);
        animator.SetBool("isWalking", walking);

        
        Vector3 move = transform.right * x + transform.forward * z;
        
        if (walking && Input.GetKey(KeyCode.LeftShift))
        {
            charController.Move(move * (speed * 2) * Time.deltaTime);
            animator.SetBool("isRunning", true);
        }
        else if (walking) 
        {
            charController.Move(move * speed * Time.deltaTime);
            animator.SetBool("isRunning", false);
        }

        velocity.y -= gravity * Time.deltaTime;

        charController.Move(velocity * Time.deltaTime);
    }

}
