using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Pawn
{
    public Explosions explosion;
    private bool[] spawned = new bool[2];

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

    public override void LoseHealth(int amount)
    {
        base.LoseHealth(amount);

        if (explosion == null)
            return;

        if (explosion.slimeCapsule != null)
        {
            if (curHealth < maxHealth / 3 && !spawned[1])
                spawned[1] = true;
            else if (curHealth < maxHealth / 3 && !spawned[0])
                spawned[0] = true;
            else
                return;

            GameObject insta = Instantiate(explosion.slimeCapsule, transform.position + Vector3.up, Quaternion.identity);

            if (insta.GetComponent<ArrowController>() != null)
                insta.GetComponent<ArrowController>().side = side;

            if (insta.GetComponent<Rigidbody2D>() != null)
            {
                Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 1f)).normalized;
                insta.GetComponent<Rigidbody2D>().AddForce(dir * explosion.force);
            }
        }
    }
}
