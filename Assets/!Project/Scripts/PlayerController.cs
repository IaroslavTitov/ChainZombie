using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerController : CharacterController
{
    [Header("Attack Logic")]
    public GameObject attackConeWrapper;
    public Collider2D attackCone;
    public float attackCooldown;
    public float attackTimer;

    [Header("Health Logic")]
    public int maxHP;
    public float knockback;

    [Header("Object Links")]
    public GameObject chainlinkPrefab;
    public Rigidbody2D zombieBuddy;
    public GameObject[] spawnPoints;
    public float zombieBuddyDistance;

    private int hp;
    private float attackCooldownLeft = 0;
    private bool hasAttacked;
    private ScoreSystem scoreSystem;
    private SoundManager soundManager;
    private bool chainGenerated;

    private void Awake()
    {
        transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        zombieBuddy.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * zombieBuddyDistance;
    }

    private void Start()
    {
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        scoreSystem.hpText.text = maxHP + " HP";
        hp = maxHP;
    }

    void Update()
    {
        if (!chainGenerated)
        {
            chainGenerated = true;
            ChainGenerator.GenerateChain(chainlinkPrefab, rigidbody, zombieBuddy, Vector2.up * 0.5f, Vector2.up * 0.5f, float.PositiveInfinity);
        }

        if (hp > 0)
        {
            GatherInput();
        }
        else
        {
            moveInput = Vector2.zero;
        }

        ApplyInput();

        if (hp <= 0)
        {
            return;
        }

        animator.SetBool("isMoving", rigidbody.velocity != Vector2.zero);

        TurnAttackCone();

        // Can't atack during cooldown
        if (attackCooldownLeft > 0)
        {
            AttackCooldown();
        }
        else
        {
            CheckToAttack();
        }
    }

    private void TurnAttackCone()
    {
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg + 90;
            attackConeWrapper.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void GatherInput()
    {
        // Stop if attacking
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            moveInput = Vector2.zero;
        }
        else
        {
            moveInput = new Vector2(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"));
        }
    }

    private void CheckToAttack()
    {
        if (SimpleInput.GetButtonDown("Space"))
        {
            attackCooldownLeft = attackCooldown;
            hasAttacked = false;

            animator.SetTrigger("Attack");
        }
    }

    private void AttackCooldown()
    {
        attackCooldownLeft -= Time.deltaTime;

        if (attackCooldownLeft < attackTimer && !hasAttacked)
        {
            hasAttacked = true;

            List<Collider2D> results = new List<Collider2D>();
            if (attackCone.OverlapCollider(new ContactFilter2D(), results) > 0)
            {
                var enemies = results.Where(x => x.gameObject.layer == LayerMask.NameToLayer("Enemies") || x.gameObject.layer == LayerMask.NameToLayer("StunnedZombie"));
                foreach (var collider in enemies)
                {
                    ZombieScript zombie = collider.GetComponent<ZombieScript>();
                    Vector2 direction = (collider.attachedRigidbody.position - rigidbody.position).normalized;
                    zombie.Hit(direction);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (hp > 0)
            {
                hp--;
                scoreSystem.hpText.text = hp + " HP";
                Vector2 direction = (rigidbody.position - collision.rigidbody.position).normalized;
                rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
                Hit();
                soundManager.playSoundEffect(soundManager.playerHurtSound);

                if (hp <= 0)
                {
                    GameObject.FindObjectOfType<ScoreSystem>().GameOver();
                }
            }
        }
    }
}
