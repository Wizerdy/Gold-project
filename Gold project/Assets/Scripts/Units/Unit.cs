using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public abstract class Unit : MonoBehaviour
{
    public enum Type { PAWN, STRUCTURE };
    public enum Side { NEUTRAL, ALLY, ENEMY };

    [Header("States")]
    [HideInInspector] public Type type;
    public Side side;

    [Header("Stats")]
    public int maxHealth;
    public Vector2Int damage;
    public float attackSpeed;
    public Colors color;
    [SerializeField] private Collider2D attackRange = null;
    [SerializeField] private SpriteRenderer sprRend = null;

    [HideInInspector] public bool canAttack;
    [HideInInspector] protected int curHealth;
    [HideInInspector] public List<Collider2D> hit;
    public int cost;
    protected int hitIndex;
    protected ContactFilter2D attackFilter;
    protected bool stunt;

    public Unit(Type type) { this.type = type; }

    protected virtual void Start()
    {
        curHealth = maxHealth;
        canAttack = true;
        stunt = false;

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
        GameManager.instance.SpawnSplash(transform.position);
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
        if (sprRend != null)
            StartCoroutine(Coloration(Color.red, 0.05f));
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

    public IEnumerator DOT(int damage, float duration, float cd)
    {
        float time = 0;
        while(time < duration)
        {
            LoseHealth(damage);
            yield return new WaitForSeconds(cd);
            time += cd;
        }
    }

    public IEnumerator Stunt(float duration)
    {
        stunt = true;
        yield return new WaitForSeconds(duration);
        stunt = false;
    }
}
