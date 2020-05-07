using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public enum Type { PAWN, STRUCTURE };
    public enum Side { ALLY, ENEMY, NEUTRAL };

    [Header("States")]
    [HideInInspector] public Type type;
    [HideInInspector] public Side side;

    [Header("Stats")]
    public int maxHealth;
    public int damage;
    public float attackSpeed;
    public Color color;
    [SerializeField] private Collider2D attackRange = null;

    [HideInInspector] public bool canAttack;
    protected int curHealth;
    protected List<Collider2D> hit;
    protected int hitIndex;
    protected ContactFilter2D attackFilter;

    public Unit(Type type) { this.type = type; }

    protected virtual void Start()
    {
        curHealth = maxHealth;
        canAttack = true;

        hit = new List<Collider2D>();
        attackFilter = new ContactFilter2D();
        attackFilter.layerMask = GameManager.instance.unitLayer;
        attackFilter.useLayerMask = true;
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
        target.GetComponent<Unit>().LoseHealth(damage);
    }

    protected virtual void LoseHealth(int amount)
    {
        curHealth -= amount;
    }

    protected void LateUpdate()
    {
        if (curHealth <= 0)
            Destroy(gameObject);
    }
}
