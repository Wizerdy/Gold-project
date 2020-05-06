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
    public Color color;

    protected int curHealth;

    public Unit(Type type) { this.type = type; }

    private void Start()
    {
        curHealth = maxHealth;
    }
}
