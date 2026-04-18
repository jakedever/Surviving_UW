using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;
    
    UnityEngine.Vector2 knockbackVelocity;
    float knockbackDuration;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject)
        {
            if (knockbackDuration > 0)
        {
            transform.position += (UnityEngine.Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        // Constantly move towards player  
        else 
        {
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
        }   
        }
    }

    public void Knockback (UnityEngine.Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        // Begins knockback
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
