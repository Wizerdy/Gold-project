using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : Unit
{
    public float speed;
    private float curSpeed;
    private Dictionary<float, int> slows;

    [Header("Other")]
    public Transform target;

    public Pawn() : base(Type.PAWN) { }

    protected override void Start()
    {
        base.Start();

        curSpeed = speed;
        slows = new Dictionary<float, int>();
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
            Vector3 dir = (target.transform.position - transform.position).normalized * curSpeed * Time.deltaTime;
            //transform.Translate(new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y, transform.position.z + dir.z);
            Debug.DrawLine(transform.position, transform.position + dir / (Time.deltaTime * curSpeed), Color.red, Time.deltaTime);
        }
    }

    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        target.GetComponent<Unit>().LoseHealth(DealDamage());
    }

    protected override void OnDestroy()
    {

    }

    public int AddSlow(float slow)
    {
        slows[slow]++;

        if(TestSlow(slow))
        {
            curSpeed = speed * (1 - slow);
        }

        return slows.Count - 1;
    }

    public void RemSlow(float slow)
    {
        if(slows[slow] > 0)
        {
            slows[slow]--;

            if(slows[slow] == 0)
                foreach(int value in slows.Values)
                    if(TestSlow(value))
                        curSpeed = speed * (1 - value);
        }
    }

    public void Burn(float damage, float time)
    {

    }

    private bool TestSlow(float slow)
    {
        if (speed * (1 - slow) < curSpeed)
        {
            return true;
        }
        return false;
    }

    //IEnumerator TakeDamage(float time)
    //{

    //}

    //private void SlowAdd(float slow)
    //{
    //    if(slows[slow] == 0)
    //    {
    //        slows.Add(slow, 1);
    //    } else
    //    {
    //        slows[slow]++;
    //    }
    //}
}
