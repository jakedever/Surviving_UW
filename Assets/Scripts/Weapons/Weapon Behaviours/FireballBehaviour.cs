using UnityEngine;

public class FireballBehaviour : ProjectileWeaponBehaviour
{
    public FireballController fireballController;
    public GameObject fireballAOE;
    protected override void Start()
    {
        base.Start();
        fireballController = FindFirstObjectByType<FireballController>();
        fireballAOE = fireballController.weaponData.AdditionalObjects[0];
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
            fireballController.SpawnAOE(col.GetComponent<Transform>().position);
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage()); // Make sure to use current damage incase modifiers

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
