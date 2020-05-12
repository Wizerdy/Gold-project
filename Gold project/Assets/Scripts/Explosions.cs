using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Explosion", menuName = "ScriptableObjects/Explosion", order = 1)]
public class Explosions : ScriptableObject
{
    public float lifetime;
    [HideInInspector] public Unit.Side side;

    public int damage;
    public float damageSpeed;

    [Range(0.0f, 1.0f)] public float slow;
    public bool stunt;
    public bool burn;
    public bool poison;
}
