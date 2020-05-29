using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
    [HideInInspector] public Side lastDamageSide;

    public Transform spawnPoint;
    public List<Transform> turrets;
    public List<Transform> pumps;

    protected override void Start()
    {
        base.Start();
        Touched touch = GetComponent<Touched>();
        if (side == Side.ALLY && touch != null)
            touch.active = true;
            
    }

    protected override void Die()
    {
        //for (int i = 1; i < transform.childCount; i++)
            //Destroy(transform.GetChild(i).gameObject);

        side = lastDamageSide;

        for (int i = 0; i < turrets.Count; i++)
            for (int j = 0; j < turrets[i].childCount; j++)
            {
                Destroy(turrets[i].GetChild(j).gameObject);
                if(side == Side.ENEMY)
                    GameManager.instance.iA.turretCount--;
            }

        for (int i = 0; i < pumps.Count; i++)
        {
            pumps[i].gameObject.SetActive(false);
            pumps[i].GetComponent<Pump>().enabled = true;
            if (side == Side.ENEMY)
                GameManager.instance.iA.pumpCount--;
        }


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
