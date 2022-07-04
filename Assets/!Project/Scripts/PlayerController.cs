using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CharacterController
{
    public GameObject chainlinkPrefab;
    public Rigidbody2D test1;
    public Rigidbody2D test2;
    public Rigidbody2D test3;
    public Rigidbody2D test4;
    public GameObject attackConeWrapper;
    public Collider2D attackCone;

    public float attackCooldown;
    public float attackTimer;
    private float attackCooldownLeft = 0;
    private bool hasAttacked;



    private void Start()
    {
        ChainGenerator.GenerateChain(chainlinkPrefab, test3, test4, Vector2.zero, Vector2.up * 0.5f);
        ChainGenerator.GenerateChain(chainlinkPrefab, test1, test2, Vector2.up * 0.5f, Vector2.up * 0.5f);
    }
    void Update()
    {
        GatherInput();

        ApplyInput();

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
}
