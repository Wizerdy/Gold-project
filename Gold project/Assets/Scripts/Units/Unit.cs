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

    [HideInInspector] public bool canAttack;
    protected int curHealth;

    public Unit(Type type) { this.type = type; }

    protected virtual void Start()
    {
        curHealth = maxHealth;
    }

    protected IEnumerator AtkCountdown(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }
}
