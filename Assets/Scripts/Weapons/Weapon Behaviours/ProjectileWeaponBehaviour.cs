using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{

    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    // Current Stats
    protected float currentDamage;
    protected float currentSpeed; 
    protected float currentCooldownDuration;
    protected int currentPierce;

    private void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    } 

    public float GetCurrentDamage()
    {
        return currentDamage += FindObjectOfType<PlayerStats>().CurrentMight;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.localRotation.eulerAngles;

        // BASE MODEL IS RIGHT-FACING
        if (dirx < 0 && diry == 0) // left
        {
            scale.x *= -1;
            scale.y *= -1;
        }
        else if (dirx == 0 && diry > 0) // down
        {
            scale.y *= -1;
        }
        else if (dirx < 0 && diry == 0) // up
        {
            scale.x *= -1;
        }
        else if (dirx < 0 && diry > 0) // right up
        {
            rotation.z = 0f;
        }
        else if (dirx < 0 && diry == 0) // right down
        {
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry > 0) // left up
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry < 0) // left down
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;
        }
   

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }


    protected virtual void OnTriggerEnter2D(Collider2D col) 
    {    
        if (col.CompareTag("enemy"))   
        {
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

    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce == 0)
        {
            Destroy(gameObject); 
        }
    }


}
