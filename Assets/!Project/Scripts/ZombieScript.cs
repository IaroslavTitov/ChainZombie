using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : CharacterController
{
    private Rigidbody2D target;

    public float firstKnockback;
    public float secondKnockback;
    public float firstStun;
    public float secondStun;
    public float firstEffectTime;
    public float secondEffectTime;

    [Range(0, 1)]
    public float firstSlow;
    [Range(0, 1)]
    public float secondSlow;

    private float stunTime;
    private float effectTime;
    private ZombieState state;
    private float maxFullSpeed;

    private void Start()
    {
        target = GameObject.FindObjectOfType<PlayerController>().rigidbody;
        maxFullSpeed = maxSpeed;
    }

    void Update()
    {
        if (effectTime >= 0)
        {
            effectTime -= Time.deltaTime;

            if (effectTime <= 0)
            {
                if (state == ZombieState.STUNNED)
                {
                    maxSpeed = maxFullSpeed * firstSlow;
                    state = ZombieState.SLOW;
                    effectTime = firstEffectTime;
                }
                else
                if (state == ZombieState.SLOW)
                {
                    maxSpeed = maxFullSpeed;
                    state = ZombieState.NORMAL;
                    effectTime = 0;
                }
            }
        }

        if (stunTime <= 0)
        {
            moveInput = (this.target.position - rigidbody.position).normalized;
            animator.SetBool("Stun", false);
        }
        else
        {
            stunTime -= Time.deltaTime;
            moveInput = Vector2.zero;
            animator.SetBool("Stun", true);
        }

        ApplyInput();
    }

    public void Hit(Vector2 direction)
    {
        base.Hit();

        if (state == ZombieState.NORMAL)
        {
            stunTime = firstStun;
            effectTime = firstEffectTime;
            rigidbody.AddForce(direction * firstKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * firstSlow;
            state = ZombieState.SLOW;
        }
        else if (state == ZombieState.SLOW)
        {
            stunTime = secondStun;
            effectTime = secondEffectTime;
            rigidbody.AddForce(direction * secondKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * secondSlow;
            state = ZombieState.STUNNED;
        }
        else if (state == ZombieState.STUNNED)
        {
            stunTime = secondStun;
            effectTime = secondEffectTime;
            rigidbody.AddForce(direction * secondKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * secondSlow;
            state = ZombieState.STUNNED;
        }
    }

    public enum ZombieState
    {
        NORMAL,
        SLOW,
        STUNNED
    }
}
