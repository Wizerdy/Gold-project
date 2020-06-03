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
    protected ContactFilter2D attackFilter;

    protected Coroutine stunt;
    protected bool stunned;

    protected Coroutine dot;
    protected int dotDamage;

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

    protected bool CheckAttackRange()
    {
        if (attackRange.OverlapCollider(attackFilter, hit) > 0)
            for (int i = 0; i < hit.Count; i++)
                if (hit[i] != null && hit[i].GetComponent<Unit>().side != side)
                {
                    hitIndex = i;
                    return true;
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

    public virtual void AddDoT(int damage, float duration)
    {
        StartCoroutine(IntensifyDoT(damage, duration));

        if (dot == null)
            dot = StartCoroutine("DOT");
    }

    public virtual void Stunt(float duration)
    {
        if (stunt != null)
            StopCoroutine(stunt);

        stunt = StartCoroutine(Stunned(duration));
    }

    protected IEnumerator IntensifyDoT(int damage, float duration)
    {
        dotDamage += damage;
        yield return new WaitForSeconds(duration);
        dotDamage -= damage;
    }

    protected IEnumerator DOT()
    {
        while(true)
        {
            yield return new WaitForSeconds(GameManager.instance.dotSpeed);
            LoseHealth(dotDamage);
        }
    }

    protected IEnumerator Stunned(float duration)
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
    }
}
