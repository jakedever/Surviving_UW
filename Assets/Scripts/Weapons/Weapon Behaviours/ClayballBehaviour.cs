using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayballBehaviour : ProjectileWeaponBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * currentSpeed * direction;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col) 
    {   
        if (col.CompareTag("enemy"))   
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage()); // Make sure to use current damage incase modifiers
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
