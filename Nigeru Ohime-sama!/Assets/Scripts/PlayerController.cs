using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Movement speed of the player

    private Rigidbody2D rb; // Reference to Rigidbody2D component
    private Animator anims;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAnims();
    }
    
    private void FixedUpdate()
    {
        // Get the horizontal and vertical input (from keyboard or controller)
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector2 moveDirection = new Vector2(horizontalMovement, verticalMovement).normalized;

        // Move the player
        rb.velocity = moveDirection * moveSpeed;
        // rb.AddForce(moveDirection * moveSpeed);
    }

    private void HandleAnims()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool isRight = Input.GetKey(KeyCode.D);
        bool isLeft = Input.GetKey(KeyCode.A);
        bool isUp = Input.GetKey(KeyCode.W);
        bool isDown = Input.GetKey(KeyCode.S);

        anims.SetBool("isMoving", isMoving);
        anims.SetBool("isRight", isRight);
        anims.SetBool("isLeft", isLeft);
        anims.SetBool("isUp", (isUp && !isRight && !isLeft));
        anims.SetBool("isDown", (isDown && !isRight && !isLeft));
    }
}
