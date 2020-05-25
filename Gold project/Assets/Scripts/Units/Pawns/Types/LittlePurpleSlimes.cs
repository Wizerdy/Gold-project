using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlePurpleSlimes : Pawn
{
    public float lifeTime;

    protected override void Start()
    {
        base.Start();
        StartCoroutine("Live", lifeTime);
    }

    IEnumerator Live(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
