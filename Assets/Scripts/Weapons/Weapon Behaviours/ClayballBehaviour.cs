using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayballBehaviour : ProjectileWeaponBehaviour
{
    // Transform hitLocation = new Transform();
    // Start is called before the first frame update
    public ClayballController clayballController;
    protected override void Start()
    {
        base.Start();
        clayballController = FindFirstObjectByType<ClayballController>();
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
            UnityEngine.Debug.Log(currentMiscellaneous);
            
            if (currentMiscellaneous > 0) // If there is still a level of spliting that needs to occur
            {
                UnityEngine.Debug.Log(currentMiscellaneous);
                currentMiscellaneous--;
                clayballController.AttackAndSplit(direction, gameObject.transform, (int)currentMiscellaneous);
            }
            
            ReducePierce(); // Since Clayball has one pierce, this will delete the clayball

            
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
