using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Explosion", menuName = "ScriptableObjects/Explosion", order = 1)]
public class Explosions : ScriptableObject
{
    public Vector2 size;

    public int damage;

    [Header("Stunt")]
    public bool stunt;
    public float stuntDuration;

    [Header("Poison")]
    public int dot;
    public float dotDuration;

    [Header("Slow")]
    [Range(0.0f, 1.0f)] public float slow;
    public float slowTime;

    [Header("Little")]
    public bool little;
    public GameObject littleslime;
}
