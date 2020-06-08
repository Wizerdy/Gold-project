using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public abstract class Unit : MonoBehaviour
{
    public enum Type { PAWN, STRUCTURE };
    public enum Side { NEUTRAL, ALLY, ENEMY };

    [Header("Descripton")]
    public string surname;
    [TextArea(0, 3)] public string description;

    [Header("States")]
    [HideInInspector] public Type type;
    public Side side;

    [Header("Stats")]
    public int maxHealth;
    public Vector2Int damage;
    public float attackSpeed;
    public Colors color;
    [SerializeField] private Collider2D attackRange = null;
    [SerializeField] protected SpriteRenderer sprRend = null;

    [HideInInspector] public bool canAttack;
    [HideInInspector] public int curHealth;
    [HideInInspector] public List<Collider2D> hit;
    public int cost;
    protected int hitIndex;
    protected GameObject hitTarget;
    protected ContactFilter2D attackFilter;

    protected Coroutine stunt;
    protected bool stunned;

    protected Coroutine poison;
    protected float poisonDamage;

    protected Coroutine burn;
    protected int burnDamage;

    protected GameObject burnParticle;
    protected GameObject regenParticle;

    public Unit(Type type) { this.type = type; }

    protected virtual void Start()
    {
        curHealth = maxHealth;
        canAttack = true;
        stunned = false;

        hit = new List<Collider2D>();
        attackFilter = new ContactFilter2D
        {
            layerMask = GameManager.instance.unitLayer,
            useLayerMask = true
        };

        if (sprRend == null)
            sprRend = GetComponent<SpriteRenderer>();
    }

    protected void LateUpdate()
    {
        if (maxHealth > 0 && curHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected IEnumerator AtkCountdown(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    protected virtual bool CheckAttackRange()
    {
        if (attackRange.OverlapCollider(attackFilter, hit) > 0)
        {
            for (int i = 0; i < hit.Count; i++)
                if (hit[i] != null && hit[i].gameObject == hitTarget && hitTarget.GetComponent<Unit>().side != side)
                {
                    hitIndex = i;
                    return true;
                }

            for (int i = 0; i < hit.Count; i++)
                if (hit[i] != null && hit[i].GetComponent<Unit>().side != side)
                {
                    hitIndex = i;
                    hitTarget = hit[i].gameObject;
                    return true;
                }
        }
        return false;
    }

    protected virtual void Attack(GameObject target)
    {
        StartCoroutine(AtkCountdown(attackSpeed));
    }

    public virtual void LoseHealth(int amount)
    {
        curHealth -= amount;
        //if (sprRend != null)
        //StartCoroutine(Coloration(Color.red, 0.05f));

        if (curHealth < 0)
            curHealth = 0;
    }

    public virtual void LoseHealth(float amount)
    {
        LoseHealth(maxHealth * amount);
    }

    protected IEnumerator Coloration(Color color, float time)
    {
        Color baseColor = sprRend.color;
        sprRend.color = color;
        yield return new WaitForSeconds(time);
        sprRend.color = baseColor;
    }

    public int DealDamage()
    {
        return Random.Range(damage.x, damage.y);
    }

    protected virtual void OnDestroy()
    {
        
    }

    public virtual void Stunt(float duration)
    {
        if (stunt != null)
            StopCoroutine(stunt);

        stunt = StartCoroutine(Stunned(duration));
    }

    protected IEnumerator Stunned(float duration)
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    #region DoTs

    public virtual void AddPoison(float damage, float duration)
    {
        StartCoroutine(IntensifyPoison(damage, duration));

        if (poison == null)
            poison = StartCoroutine("Poison");
    }

    public virtual void AddBurn(int damage, float duration)
    {
        StartCoroutine(IntensifyBurn(damage, duration));

        if (burn == null)
            burn = StartCoroutine("Burn");
    }

    protected IEnumerator IntensifyPoison(float damage, float duration)
    {
        poisonDamage += damage;
        yield return new WaitForSeconds(duration);
        poisonDamage -= damage;

        if (poisonDamage < 0)
            poisonDamage = 0;
    }

    protected IEnumerator IntensifyBurn(int damage, float duration)
    {
        burnDamage += damage;
        yield return new WaitForSeconds(duration);
        burnDamage -= damage;


        if (burnDamage < 0)
            burnDamage = 0;
    }

    protected IEnumerator Poison()
    {
        while(true)
        {
            yield return new WaitForSeconds(GameManager.instance.dotSpeed);
            LoseHealth(poisonDamage);
        }
    }

    protected IEnumerator Burn()
    {
        if (burnParticle == null)
            burnParticle = Instantiate(GameManager.instance.burnParticles, transform);

        while (true)
        {
            if (burnDamage > 0)
            {
                if (!burnParticle.activeSelf)
                    burnParticle.SetActive(true);

                yield return new WaitForSeconds(GameManager.instance.dotSpeed);
                LoseHealth(burnDamage);
            } else
            {
                if (burnParticle.activeSelf)
                    burnParticle.SetActive(false);
            }
        }
    }

    #endregion
}
