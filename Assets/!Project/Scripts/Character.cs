using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Base System")]
    public new Rigidbody2D rigidbody;
    public Animator animator;
    public float acceleration;
    public float maxSpeed;
    public float minSpeed;
    public float friction;
    public ParticleSystem hurtParticles;

    private Vector2 lastDirection;
    protected Vector2 moveInput = Vector2.zero;

    protected void ApplyInput()
    {
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
                rigidbody.velocity = Vector2.zero;
            }
        }

        if (moveInput != Vector2.zero)
            lastDirection = moveInput.normalized;

        //Set animator vars
        animator.SetFloat("X", lastDirection.x);
        animator.SetFloat("Y", lastDirection.y);
    }

    public void Hit()
    {
        hurtParticles.Play();
    }
}
