using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCaps : ArrowController
{
    [SerializeField] private GameObject lilPurp;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        
        if (GameManager.InsideLayer(go.layer, GameManager.instance.floorLayer))
        {
            if (behaviour != null && explode)
            {
                GameObject insta = Instantiate(GameManager.instance.explosion, transform.position, Quaternion.identity);
                insta.GetComponent<EffectZone>().behaviour = behaviour;
            }

            GameManager.instance.InstantiateUnit(lilPurp, side, transform.position);

            Destroy(gameObject);
        }
    }
}
