using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : Unit
{
    public float speed;
    [SerializeField] protected float curSpeed;
    public int regen;
    public float regenCd;

    private Dictionary<float, int> slows;

    protected Vector3 baseScale;

    [HideInInspector] public bool immobilize;

    [Header("Attack")]
    [SerializeField] private Explosions atkBehaviour;

    [Header("Immunities")]
    public bool imuExplosion;
    [SerializeField] private bool imuSlow;
    [SerializeField] private bool imuStunt;
    [SerializeField] private bool imuPoison;
    [SerializeField] private bool imuBurn;

    [Header("Other")]
    public Transform target;

    private Coroutine atkPrep;

    public Pawn() : base(Type.PAWN) { }

    protected override void Start()
    {
        base.Start();

        curSpeed = speed;
        slows = new Dictionary<float, int>();

        baseScale = transform.localScale;

        immobilize = false;

        if(sprRend != null)
            sprRend.transform.localScale = new Vector3(Mathf.Sqrt((float)maxHealth / 50f), Mathf.Sqrt((float)maxHealth / 50f), 1);

        canAttack = false;

        if (regen > 0)
            StartCoroutine("Regen");
    }

    private void Update()
    {
        if (stunned && !imuStunt)
            return;

        if (!CheckAttackRange())
        {
            Move();
            immobilize = false;
        }
        else if (canAttack)
        {
            Attack(hit[hitIndex].gameObject);
            immobilize = true;
            canAttack = false;
        }
        else if (atkPrep == null)
        {
            atkPrep = StartCoroutine(AtkCountdown(attackSpeed));
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

        Unit unit = target.GetComponent<Unit>();

        if (atkBehaviour != null)
        {

            if (target.GetComponent<Pawn>() != null)
            {
                Pawn pawn = target.GetComponent<Pawn>();

                if (!pawn.imuExplosion)
                    unit.LoseHealth(atkBehaviour.damage);

                if (atkBehaviour.slow > 0)
                    if (atkBehaviour.slowTime > 0)
                        pawn.AddSlow(atkBehaviour.slow, atkBehaviour.slowTime);
                    else
                        pawn.AddSlow(atkBehaviour.slow);

                if (atkBehaviour.stunt)
                    unit.Stunt(atkBehaviour.stuntDuration);
            }

            if (atkBehaviour.poison > 0)
                unit.AddPoison(atkBehaviour.poison, atkBehaviour.poisonDuration);

            if (atkBehaviour.burn > 0)
                unit.AddBurn(atkBehaviour.burn, atkBehaviour.burnDuration);

        }

        unit.LoseHealth(DealDamage());

        if (target.GetComponent<Tower>() != null)
            target.GetComponent<Tower>().lastDamageSide = side;
    }

    public override void LoseHealth(int amount)
    {
        base.LoseHealth(amount);

        transform.localScale = Tools.Map(curHealth, 0, maxHealth, baseScale * GameManager.instance.slimeMinSize, baseScale);
    }

    public override void AddPoison(float damage, float duration)
    {
        if (imuPoison)
            return;

        base.AddPoison(damage, duration);
    }

    public override void AddBurn(int damage, float duration)
    {
        if (imuBurn)
            return;

        base.AddBurn(damage, duration);
    }

    public override void Stunt(float duration)
    {
        if (imuStunt)
            return;

        base.Stunt(duration);
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

    protected IEnumerator Regen()
    {
        while (curHealth > 0)
        {
            if (curHealth - regen <= maxHealth)
                curHealth += regen;
            else
                curHealth = maxHealth;
            yield return new WaitForSeconds(regenCd);
        }
    }

    #region Slow

    public void AddSlow(float slow)
    {
        if (imuSlow)
            return;

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
