using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : Unit
{
    public float speed;

    [Header("Other")]
    public Transform target;
    [SerializeField] private Collider2D attackRange;
    [SerializeField] private LayerMask unitLayer;

    private Collider2D[] hit;
    private ContactFilter2D attackFilter;

    public Pawn() : base(Type.PAWN) { }

    protected override void Start()
    {
        base.Start();

        hit = new Collider2D[5];
        attackFilter = new ContactFilter2D();
        attackFilter.layerMask = unitLayer;
        attackFilter.useLayerMask = true;
    }

    private void Update()
    {
        if(!CheckAttackRange()) {
            Move();
        } else if(canAttack)
        {
            Attack(hit[0].gameObject);
        }
    }

    public void Move()
    {
        if (target != null)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
            //transform.Translate(new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y, transform.position.z + dir.z);
            Debug.DrawLine(transform.position, transform.position + dir, Color.red, Time.deltaTime);
        }
    }

    public bool CheckAttackRange() {
        if(attackRange.OverlapCollider(attackFilter, hit) > 0)
        {
            return true;
        }
        return false;
    }

    public void Attack(GameObject target)
    {
        StartCoroutine(AtkCountdown(attackSpeed));
    }
}
