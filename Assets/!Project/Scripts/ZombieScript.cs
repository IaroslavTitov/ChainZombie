using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Header("Yell Frequency")]
    public float minYellPeriod;
    public float maxYellPeriod;

    [Header("Score")]
    public GameObject pointNotification;

    [Header("Animators")]
    public RuntimeAnimatorController[] normalAnimators;
    public RuntimeAnimatorController[] gachiAnimators;

    private float maxFullSpeed;
    private float stunTime;
    private float effectTime;
    private float angle = 0;
    private ZombieState state;
    private Rigidbody2D target;
    private SoundManager soundManager;
    private float yellTimer;
    private ScoreSystem scoreSystem;
    private void Start()
    {
        target = GameObject.FindObjectOfType<PlayerController>()?.rigidbody;
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        maxFullSpeed = maxSpeed;
        yellTimer = Random.Range(minYellPeriod, maxYellPeriod);
        animator = GetComponent<Animator>();

        SetGachi();
    }

    void Update()
    {
        // Timeout Effects
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

        // Stun timeout
        if (stunTime <= 0)
        {
            if (target != null)
                moveInput = (this.target.position - rigidbody.position).normalized;
            else
                moveInput = Vector2.zero;

            animator.SetBool("Stun", false);
            if (gameObject.layer == LayerMask.NameToLayer("StunnedZombie"))
            {
                gameObject.layer = LayerMask.NameToLayer("Enemies");
            }
        }
        else
        {
            stunTime -= Time.deltaTime;
            moveInput = Vector2.zero;
            animator.SetBool("Stun", true);
        }


        // Switch to roam
        if (target == null || Vector2.Distance(this.target.position, rigidbody.position) > sightDistance)
        {
            angle += Random.Range(-1f, 1f) * roamRotation;
            moveInput = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }
        else
        {
            // Yell logic
            if (stunTime <= 0)
            {
                yellTimer -= Time.deltaTime;

                if (yellTimer <= 0)
                {
                    yellTimer = Random.Range(minYellPeriod, maxYellPeriod);
                    soundManager.playSoundEffect(soundManager.zombieYellSound);
                }
            }
        }

        ApplyInput();
    }

    public void Hit(Vector2 direction)
    {
        base.Hit();
        soundManager.playSoundEffect(soundManager.zombieHurtSound);
        scoreSystem.ZombieHit();
        PointNotificationScript.SpawnNotification(pointNotification, transform.position, scoreSystem.scorePerHit);

        if (state == ZombieState.NORMAL)
        {
            stunTime = firstStun;
            gameObject.layer = LayerMask.NameToLayer("StunnedZombie");
            effectTime = firstEffectTime;
            rigidbody.AddForce(direction * firstKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * firstSlow;
            state = ZombieState.SLOW;
        }
        else if (state == ZombieState.SLOW)
        {
            stunTime = secondStun;
            gameObject.layer = LayerMask.NameToLayer("StunnedZombie");
            effectTime = secondEffectTime;
            rigidbody.AddForce(direction * secondKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * secondSlow;
            state = ZombieState.STUNNED;
        }
        else if (state == ZombieState.STUNNED)
        {
            stunTime = secondStun;
            gameObject.layer = LayerMask.NameToLayer("StunnedZombie");
            effectTime = secondEffectTime;
            rigidbody.AddForce(direction * secondKnockback, ForceMode2D.Impulse);
            maxSpeed = maxFullSpeed * secondSlow;
            state = ZombieState.STUNNED;
        }
    }

    public void SetGachi()
    {
        if (PlayerPrefs.GetInt("RightVersion") == 0)
        {
            animator.runtimeAnimatorController = normalAnimators[Random.Range(0, normalAnimators.Length)];
        }
        else
        {
            animator.runtimeAnimatorController = gachiAnimators[Random.Range(0, gachiAnimators.Length)];
        }
    }

    public enum ZombieState
    {
        NORMAL,
        SLOW,
        STUNNED
    }
}
