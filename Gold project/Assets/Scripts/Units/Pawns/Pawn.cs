using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : Unit
{
    public float speed;

    [Header("Other")]
    public Transform target;

    public Pawn() : base(Type.PAWN) { }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if(!CheckAttackRange()) {
            Move();
        } else if(canAttack)
        {
            Attack(hit[hitIndex].gameObject);
        }
    }

    public void Move()
    {
        if (target != null)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
            //transform.Translate(new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y, transform.position.z + dir.z);
            Debug.DrawLine(transform.position, transform.position + dir / (Time.deltaTime * speed), Color.red, Time.deltaTime);
        }
    }

    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        target.GetComponent<Unit>().LoseHealth(damage);
    }

}
