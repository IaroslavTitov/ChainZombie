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
                    Debug.Log(name + " recovered from STUNNED state, now he's SLOW");
                }
                else
                if (state == ZombieState.SLOW)
                {
                    maxSpeed = maxFullSpeed;
                    state = ZombieState.NORMAL;
                    effectTime = 0;
                    Debug.Log(name + " recovered from SLOW state, now he's NORMAL");
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
            Debug.Log(name + " entered a SLOW state");
        }
        else if (state == ZombieState.SLOW)
        {
            stunTime = secondStun;
            effectTime = secondEffectTime;
            rigidbody.AddForce(direction * secondKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * secondSlow;
            state = ZombieState.STUNNED;
            Debug.Log(name + " entered a STUNNED state");
        }
        else if (state == ZombieState.STUNNED)
        {
            stunTime = secondStun;
            effectTime = secondEffectTime;
            rigidbody.AddForce(direction * secondKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * secondSlow;
            state = ZombieState.STUNNED;
            Debug.Log(name + " kee[s STUNNED state");
        }
    }

    public enum ZombieState
    {
        NORMAL,
        SLOW,
        STUNNED
    }
}
