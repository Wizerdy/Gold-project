using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnim : MonoBehaviour
{
    private Animator anim;
    public int herbe = 0;

    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetInteger("isHerbe", herbe);
        anim.SetInteger("IdleIndex", Random.Range(0, 2));
    }

}
