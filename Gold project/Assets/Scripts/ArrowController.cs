using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [HideInInspector] public Unit.Side side;
    [HideInInspector] public int damage;

    public bool explode;
    public Explosions behaviour;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if(GameManager.InsideLayer(go.layer, GameManager.instance.unitLayer) &&
            go.GetComponent<Unit>().side != side &&
            go.GetComponent<Unit>().type == Unit.Type.PAWN)
        {
            Pawn unit = go.GetComponent<Pawn>();

            go.GetComponent<Pawn>().LoseHealth(damage);

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

                    if (behaviour.poison)
                        unit.StartCoroutine(unit.DOT(behaviour.dot, behaviour.dotDuration, behaviour.damageSpeed));
                }
            }

            Destroy(gameObject);
        } else if(GameManager.InsideLayer(go.layer, GameManager.instance.floorLayer))
        {
            Destroy(gameObject);
        }
    }
}
