using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : Unit
{
    public float speed;
    [SerializeField]
    protected float curSpeed;
    private Dictionary<float, int> slows;

    protected Vector3 baseScale;

    public bool immobilize;

    [Header("Other")]
    public Transform target;

    public Pawn() : base(Type.PAWN) { }

    protected override void Start()
    {
        base.Start();

        curSpeed = speed;
        slows = new Dictionary<float, int>();

        baseScale = transform.localScale;

        immobilize = false;

        if(sprRend != null)
            sprRend.transform.localScale = new Vector3(Mathf.Sqrt(maxHealth / 50f), Mathf.Sqrt(maxHealth / 50f), 1);
    }

    private void Update()
    {
        if (stunt)
            return;

        if(!CheckAttackRange()) {
            Move();
            immobilize = false;
        } else if(canAttack)
        {
            Attack(hit[hitIndex].gameObject);
            immobilize = true;
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

        if (target.GetComponent<Tower>() != null)
            target.GetComponent<Tower>().lastDamageSide = side;
    }

    public override void LoseHealth(int amount)
    {
        base.LoseHealth(amount);

        transform.localScale = Tools.Map(curHealth, 0, maxHealth, baseScale * GameManager.instance.slimeMinSize, baseScale);
    }

    protected override void Die()
    {
        GameManager.instance.SpawnSplash(transform.position, GameManager.instance.differentsColors[(int)color]);

        switch(side) {
            case Side.ALLY:
                GameManager.instance.iA.Gain(Mathf.FloorToInt(cost * GameManager.instance.slimeRefund));
                break;
            case Side.ENEMY:
                ShopManager.instance.Gain(Mathf.FloorToInt(cost * GameManager.instance.slimeRefund));
                break;
        }

        SoundManager.instance.PlaySound("Death_" + Random.Range(0, 4));

        base.Die();
    }

    #region Slow

    public void AddSlow(float slow)
    {
        if (!slows.ContainsKey(slow))
        {
            slows.Add(slow, 0);
        }

        slows[slow]++;

        if(TestSlow(slow))
        {
            curSpeed = speed * (1 - slow);
        }
    }

    public void AddSlow(float slow, float time)
    {
        AddSlow(slow);
        StartCoroutine(RemSlow(slow, time));
    }

    public void RemSlow(float slow)
    {
        if(slows[slow] > 0)
        {
            slows[slow]--;

            if (slows[slow] == 0)
            {
                foreach (KeyValuePair<float, int> pair in slows)
                    if (pair.Value > 0 && TestSlow(pair.Key))
                        curSpeed = speed * (1 - pair.Key);

                if(curSpeed == speed * (1 - slow))
                    curSpeed = speed;
            }
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

    IEnumerator RemSlow(float slow, float time)
    {
        yield return new WaitForSeconds(time);
        RemSlow(slow);
    }

    #endregion
}
