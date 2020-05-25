using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
    [HideInInspector] public Side lastDamageSide;

    protected override void Start()
    {
        base.Start();
        Touched touch = GetComponent<Touched>();
        if (side == Side.ALLY && touch != null)
            touch.active = true;
            
    }

    protected override void Die()
    {
        for (int i = 1; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        side = lastDamageSide;

        Touched touch = GetComponent<Touched>();

        switch (side)
        {
            case Side.ALLY:
                if (touch != null)
                    touch.active = true;
                GameManager.instance.allies.Add(gameObject);
                break;
            case Side.ENEMY:
                if (touch != null)
                    touch.active = false;
                break;
            case Side.NEUTRAL:
                if (touch != null)
                    touch.active = false;
                break;
        }

        curHealth = maxHealth;
    }
}
