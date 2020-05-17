using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Pawn
{
    public Explosions explosion;

    protected override void Die()
    {
        if (explosion != null)
        {
            GameObject insta = Instantiate(GameManager.instance.explosion, transform.position, Quaternion.identity);
            insta.GetComponent<EffectZone>().behaviour = explosion;
            insta.GetComponent<EffectZone>().side = side;
        }
        base.Die();
    }
}
