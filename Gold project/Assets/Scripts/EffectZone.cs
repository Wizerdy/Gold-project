﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZone : MonoBehaviour
{
    public Explosions behaviour;

    [HideInInspector] public Unit.Side side;

    private Collider2D zone;

    public List<Pawn> affected;

    public List<Collider2D> hit;
    protected ContactFilter2D attackFilter;

    private void Start()
    {
        zone = GetComponent<Collider2D>();

        float sizeX = (behaviour.size.x > 0 ? behaviour.size.x : transform.localScale.x);
        float sizeY = (behaviour.size.y > 0 ? behaviour.size.y : transform.localScale.y);
        transform.localScale = new Vector2(sizeX, sizeY);

        hit = new List<Collider2D>();
        attackFilter = new ContactFilter2D();
        attackFilter.layerMask = GameManager.instance.unitLayer;
        attackFilter.useLayerMask = true;

        if (zone.OverlapCollider(attackFilter, hit) > 0)
            for (int i = 0; i < hit.Count; i++)
                if (hit[i].GetComponent<Unit>().side != side && hit[i].GetComponent<Unit>().type == Unit.Type.PAWN)
                {
                    Attack(hit[i].GetComponent<Pawn>());
                }

        if (behaviour.lifetime > 0)
            StartCoroutine("Live", behaviour.lifetime);
        else
            Destroy(gameObject);
    }

    private void Update()
    {

    }

    IEnumerator Live(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void Attack(Pawn unit)
    {
        affected.Add(unit);

        unit.LoseHealth(behaviour.damage);
        
        if(behaviour.slow > 0)
            if(behaviour.slowTime > 0)
                unit.AddSlow(behaviour.slow, behaviour.slowTime);
            else
                unit.AddSlow(behaviour.slow);

        if (behaviour.poison)
            unit.StartCoroutine(unit.DOT(behaviour.dot, behaviour.dotDuration, behaviour.damageSpeed));

        if (behaviour.stunt)
            unit.StartCoroutine(unit.Stunt(behaviour.stuntDuration));

        if (behaviour.burn)
            unit.StartCoroutine(unit.DOT(behaviour.dot, behaviour.dotDuration, behaviour.damageSpeed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;

        if(GameManager.instance.unitLayer == (GameManager.instance.unitLayer | (1 << go.layer)) &&
            go.GetComponent<Unit>().side != side &&
            go.GetComponent<Unit>().type == Unit.Type.PAWN &&
            SearchAffected(go.GetComponent<Pawn>()) == -1)
        {
            Pawn pawn = go.GetComponent<Pawn>();
            affected.Add(pawn);


            if (behaviour.burn)
                pawn.StartCoroutine(pawn.DOT(behaviour.dot, behaviour.dotDuration, behaviour.damageSpeed));

            if (behaviour.slow > 0)
                pawn.AddSlow(behaviour.slow);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.GetComponent<Pawn>() != null ? SearchAffected(go.GetComponent<Pawn>()) >= 0 : false)
        {
            int index = SearchAffected(go.GetComponent<Pawn>());

            if (behaviour.slow > 0 && behaviour.slowTime <= 0)
                affected[index].RemSlow(behaviour.slow);

            affected.RemoveAt(index);
        }
    }

    private int SearchAffected(Pawn pawn)
    {
        for (int i = 0; i < affected.Count; i++)
        {
            if (pawn == affected[i])
                return i;
        }
        return -1;
    }
}
