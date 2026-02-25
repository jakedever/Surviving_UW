using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    Rigidbody2D rb;
    
    // [HideInInspector]
    public UnityEngine.Vector2 moveDir;
    // [HideInInspector]
    public float lastHorizontalDirection;
    // [HideInInspector]
    public float lastVerticalDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
    }

    void FixedUpdate ()
    {
        Move();
    }

    void InputManagement ()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new UnityEngine.Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalDirection = moveDir.x;
        }
        if (moveDir.y != 0)
        {
            lastVerticalDirection = moveDir.y;
        }
    }

    void Move()
    {
        rb.velocity = new UnityEngine.Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
