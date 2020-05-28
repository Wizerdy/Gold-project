﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionAct : MonoBehaviour
{
    public UnityEvent collisionEnter;
    public UnityEvent collisionStay;
    public UnityEvent collisionExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionEnter.Invoke();
        Debug.Log("non");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collisionStay.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionExit.Invoke();
        Debug.Log("oui");
    }

    public void SetHodor(bool state)
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("Hodor", state);
    }
}