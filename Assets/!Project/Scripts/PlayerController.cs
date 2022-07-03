using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public GameObject chainlinkPrefab;
    public Rigidbody2D tiedTo;

    private void Start()
    {
        ChainGenerator.GenerateChain(chainlinkPrefab, rigidbody, tiedTo);
    }
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ApplyInput();
    }
}
