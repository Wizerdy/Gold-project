using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [HideInInspector] public Unit.Side side;
    [HideInInspector] public int damage;
    [HideInInspector] public float atkPoison;

    public bool explode;
    public Explosions behaviour;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if(GameManager.InsideLayer(go.layer, GameManager.instance.unitLayer) &&
            go.GetComponent<Unit>().side != side &&
            go.GetComponent<Unit>().type == Unit.Type.PAWN)
        {
            Pawn unit = go.GetComponent<Pawn>();

            if (!unit.imuPoison && atkPoison > 0 && unit.maxHealth * atkPoison > damage)
                unit.LoseHealth(atkPoison);
            else
                unit.LoseHealth(damage);

            unit.lastDamageSide = side;

            if (behaviour != null)
            {
                if (explode)
                {
                    GameObject insta = Instantiate(GameManager.instance.explosion, transform.position, Quaternion.identity);
                    insta.GetComponent<EffectZone>().behaviour = behaviour;
                }
                else
                {
                    if (behaviour.slow > 0)
                        if (behaviour.slowTime > 0)
                            unit.AddSlow(behaviour.slow, behaviour.slowTime);
                        else
                            unit.AddSlow(behaviour.slow);

                    if (behaviour.poison > 0)
                        unit.AddPoison(behaviour.poison, behaviour.poisonDuration);

                    if (behaviour.burn > 0)
                        unit.AddBurn(behaviour.burn, behaviour.burnDuration);
                }
            }

            Destroy(gameObject);
        } else if(GameManager.InsideLayer(go.layer, GameManager.instance.floorLayer))
        {
            if (behaviour != null && explode)
            {
                GameObject insta = Instantiate(GameManager.instance.explosion, transform.position, Quaternion.identity);
                insta.GetComponent<EffectZone>().behaviour = behaviour;
            }

            Destroy(gameObject);
        }
    }
}
