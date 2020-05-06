using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : Unit
{
    public float speed;

    [Header("Other")]
    public Transform target;


    public Pawn() : base(Type.PAWN) { }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (target != null)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            transform.Translate(new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime);
        }
    }

    public void Attack() { }
}
