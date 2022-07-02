using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public Animator animator;
    public float acceleration;
    public float maxSpeed;
    public float minSpeed;
    public float friction;
    public Vector2 lastDirection;

    void Update()
    {
        ApplyInput();
    }

    private void ApplyInput()
    {
        // Read input
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Apply input
        if (moveInput != Vector2.zero)
        {
            rigidbody.AddForce(moveInput * acceleration * Time.deltaTime, ForceMode2D.Impulse);

            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            rigidbody.velocity -= rigidbody.velocity * Time.deltaTime * friction;

            if (rigidbody.velocity.magnitude < minSpeed && rigidbody.velocity != Vector2.zero)
            {
                lastDirection = rigidbody.velocity.normalized;
                rigidbody.velocity = Vector2.zero;
            }
        }

        //Set animator vars
        animator.SetFloat("X", rigidbody.velocity.x);
        animator.SetFloat("Y", rigidbody.velocity.y);
        animator.SetFloat("lastX", lastDirection.x);
        animator.SetFloat("lastY", lastDirection.y);
        animator.SetBool("isMoving", rigidbody.velocity != Vector2.zero);
    }
}
