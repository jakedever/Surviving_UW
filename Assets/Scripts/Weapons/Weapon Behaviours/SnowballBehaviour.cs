using System.Drawing;
using UnityEngine;

public class SnowballBehaviour : ProjectileWeaponBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * currentSpeed * direction;
    }

    protected override void OnTriggerEnter2D(Collider2D col) 
    {   
        if (col.CompareTag("enemy"))   
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage()); // Make sure to use current damage incase modifiers

            EnemyMovement em = col.GetComponent<EnemyMovement>(); // Add Coroutine to re-add movement component
            em.enabled = false;

            col.GetComponent<SpriteRenderer>().color = UnityEngine.Color.blue;
            ReducePierce();
        }
        else if (col.CompareTag("Props"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                currentPierce--;
            }
        }
    }
}
