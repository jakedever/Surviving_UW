using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    // References
    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;
    PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();

        // am.Play(playerStats.GetComponent<Animation>().);
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {
            am.SetBool("Move", true);
            SpriteAnimationChecker();
        }
        else
        {
            am.SetBool("Move", false);
        }
    }

    void SpriteAnimationChecker ()
    {
        if (pm.lastHorizontalDirection < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
