using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    
    // [HideInInspector]
    public UnityEngine.Vector2 moveDir;
    // [HideInInspector]
    public float lastHorizontalDirection;
    // [HideInInspector]
    public float lastVerticalDirection;
    public UnityEngine.Vector2 lastMovedVector;

    // References
    Rigidbody2D rb;
    public CharacterScriptableObject characterData;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new UnityEngine.Vector2(1, 0f);
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
            lastMovedVector = new UnityEngine.Vector2(lastHorizontalDirection, 0f);
        }
        if (moveDir.y != 0)
        {
            lastVerticalDirection = moveDir.y;
            lastMovedVector = new UnityEngine.Vector2(0f, lastVerticalDirection);
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new UnityEngine.Vector2(lastHorizontalDirection, lastVerticalDirection);
        }
    }

    void Move()
    {
        rb.velocity = new UnityEngine.Vector2(moveDir.x * characterData.MoveSpeed, moveDir.y * characterData.MoveSpeed);
    }
}
