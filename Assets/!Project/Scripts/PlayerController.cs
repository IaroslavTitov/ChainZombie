using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public GameObject chainlinkPrefab;
    public Rigidbody2D test1;
    public Rigidbody2D test2;
    public Rigidbody2D test3;
    public Rigidbody2D test4;

    private void Start()
    {
        ChainGenerator.GenerateChain(chainlinkPrefab, test3, test4, Vector2.zero, Vector2.up * 0.5f);
        ChainGenerator.GenerateChain(chainlinkPrefab, test1, test2, Vector2.up * 0.5f, Vector2.up * 0.5f);
    }
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ApplyInput();
    }
}
