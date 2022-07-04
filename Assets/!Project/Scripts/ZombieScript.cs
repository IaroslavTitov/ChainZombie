using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : CharacterController
{
    [Header("Roam")]
    public float sightDistance;
    public float roamRotation;

    [Header("Slow mode")]
    public float firstKnockback;
    public float firstStun;
    public float firstEffectTime;
    [Range(0, 1)]
    public float firstSlow;

    [Header("Stunned mode")]
    public float secondKnockback;
    public float secondStun;
    public float secondEffectTime;
    [Range(0, 1)]
    public float secondSlow;

    private float maxFullSpeed;
    private float stunTime;
    private float effectTime;
    private float angle = 0;
    private ZombieState state;
    private Rigidbody2D target;

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

        // Switch to roam
        if (Vector2.Distance(this.target.position, rigidbody.position) > sightDistance)
        {
            angle += Random.Range(-1f, 1f) * roamRotation;
            moveInput = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
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
