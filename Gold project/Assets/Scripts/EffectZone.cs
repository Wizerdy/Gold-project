using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZone : MonoBehaviour
{
    public Explosions behaviour;

    private Collider2D zone;

    public List<Pawn> affected;


    private void Start()
    {
        StartCoroutine("Live", behaviour.lifetime);
        zone = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //if(zone.OverlapCollider(attackFilter, hit) > 0)
        //    for (int i = 0; i < hit.Count; i++)
        //        if(hit[i].GetComponent<Unit>().side != behaviour.side && hit[i].GetComponent<Unit>().type == Unit.Type.PAWN)
        //        {
        //            Attack(hit[i].GetComponent<Pawn>());
        //        }
    }

    IEnumerator Live(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    //private void Attack(Pawn unit)
    //{
    //    unit.LoseHealth(behaviour.damage);
        
    //    if(behaviour.slow > 0)
    //    {
    //        unit.StartCoroutine(unit.Slow(behaviour.damageSpeed, behaviour.slow));
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if((go.layer & GameManager.instance.unitLayer) >= 1 && go.GetComponent<Unit>().side != behaviour.side && go.GetComponent<Unit>().type == Unit.Type.PAWN)
        {
            Pawn pawn = go.GetComponent<Pawn>();
            affected.Add(pawn);


            if (behaviour.damage > 0 && behaviour.damageSpeed > 0)
                DOT(pawn);

            if (behaviour.slow > 0)
                pawn.AddSlow(behaviour.slow);

            //if (behaviour.burn)
                //pawn.Burn();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
    }

    IEnumerator DOT(Pawn pawn)
    {
        while(true)
        {
            pawn.LoseHealth(behaviour.damage);
            yield return new WaitForSeconds(behaviour.damageSpeed);
        }
    }
}
