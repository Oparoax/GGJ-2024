using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Moving : NetworkBehaviour
{

    public float MoveSpeed = 5f;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        Move();
        
    }

    [Client (RequireOwnership = true)]
    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 offset = new Vector3(horizontal, Physics.gravity.y, vertical) * MoveSpeed * Time.deltaTime;

        characterController.Move(offset);
    }
}
