using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : CharacterController
{
    private Rigidbody2D target;

    private void Start()
    {
        target = GameObject.FindObjectOfType<PlayerController>().rigidbody;
    }

    void Update()
    {
        moveInput = (this.target.position - rigidbody.position).normalized;
        ApplyInput();
    }
}
