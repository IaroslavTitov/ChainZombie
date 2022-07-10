using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerController : CharacterController
{
    public GameObject attackConeWrapper;
    public Collider2D attackCone;

    public int maxHP;
    private int hp;
    private ScoreSystem scoreSystem;

    public float attackCooldown;
    public float attackTimer;
    private float attackCooldownLeft = 0;
    private bool hasAttacked;
    private SoundManager soundManager;

    public float knockback;
    public GameObject chainlinkPrefab;
    public Rigidbody2D zombieBuddy;

    private void Start()
    {
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        scoreSystem.hpText.text = maxHP + " HP";
        hp = maxHP;

        ChainGenerator.GenerateChain(chainlinkPrefab, rigidbody, zombieBuddy, Vector2.up * 0.5f, Vector2.up * 0.5f);
    }

    void Update()
    {
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
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }

    private void CheckToAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
                var enemies = results.Where(x => x.gameObject.layer == LayerMask.NameToLayer("Enemies"));
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
                soundManager.playSoundEffect(soundManager.zombieHurtSound);

                if (hp <= 0)
                {
                    GameObject.FindObjectOfType<ScoreSystem>().GameOver();
                }
            }
        }
    }
}
