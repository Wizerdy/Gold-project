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
    //[SerializeField] private Explosions atkBehaviour;
    [SerializeField, Range(0f, 1f)] private float atkPoison;
    public int thornmail;

    [Header("Immunities")]
    public bool imuExplosion;
    [SerializeField] private bool imuSlow;
    [SerializeField] private bool imuStunt;
    public bool imuPoison;
    [SerializeField] private bool imuBurn;

    [Header("Other")]
    public Transform target;
    public Transform slimeSprite;
    public Transform slimpSprite;

    private Coroutine atkPrep;

    private Animator animator;

    public Pawn() : base(Type.PAWN) { }

    protected override void Start()
    {
        base.Start();

        curSpeed = speed;
        slows = new Dictionary<float, int>();

        immobilize = false;

        if (sprRend != null)
        {
            float scale = Mathf.Pow((float)maxHealth / 400f, 1f / 3f);
            sprRend.localScale = new Vector3(scale, scale, 1);
            sprRend.localPosition = new Vector3(sprRend.localPosition.x, sprRend.localPosition.y + (scale - 1f) * 0.35f, sprRend.localPosition.z);
        }

        baseScale = transform.localScale;

        canAttack = false;

        if (regen > 0)
            StartCoroutine("Regen");

        animator = GetComponent<Animator>();

        if (side == Side.ALLY)
        {
            animator.SetBool("isAlly", true);
            slimeSprite.gameObject.SetActive(true);
            slimpSprite.gameObject.SetActive(false);
        }
        else
        {
            animator.SetBool("isAlly", false);
            slimpSprite.gameObject.SetActive(true);
            slimeSprite.gameObject.SetActive(false);
        }
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
            animator.SetTrigger("Attack");
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

        if (target.GetComponent<Pawn>() != null)
        {
            Pawn pawn = target.GetComponent<Pawn>();

            if (!pawn.imuPoison && atkPoison > 0 && pawn.maxHealth * atkPoison > DealDamage())
                pawn.LoseHealth(atkPoison);
            else
                pawn.LoseHealth(DealDamage());

            if (!imuBurn && pawn.thornmail > 0)
                LoseHealth(pawn.thornmail);
        } else
        {
            unit.LoseHealth(DealDamage());
        }

        target.GetComponent<Unit>().lastDamageSide = side;
    }

    public override void LoseHealth(int amount)
    {
        base.LoseHealth(amount);

        ChangeSize();
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

        switch(lastDamageSide) {
            case Side.ALLY:
                ShopManager.instance.Gain(Mathf.FloorToInt(cost * GameManager.instance.slimeRefund));
                break;
            case Side.ENEMY:
                GameManager.instance.iA.Gain(Mathf.FloorToInt(cost * GameManager.instance.slimeRefund));
                break;
        }

        SoundManager.instance.PlaySound("Death_" + Random.Range(0, 4));
        //GameManager.instance.SpawnDamageParticles(maxHealth, color, transform.position, side);

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

            ChangeSize();

            if (regenParticle == null)
                regenParticle = Instantiate(GameManager.instance.regenParticles, transform);

            yield return new WaitForSeconds(regenCd);
        }
    }

    protected void ChangeSize()
    {
        sprRend.transform.localScale = Tools.Map(curHealth, 0, maxHealth, baseScale * GameManager.instance.slimeMinSize, baseScale);
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
