using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThatAss : MonoBehaviour
{
    public SpriteRenderer bound;
    /*[HideInInspector]*/public Bounds boundary;
    public float speed = 1;


    protected void Start()
    {
        boundary = bound.bounds;

        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<MoveThatCloud>();
        }
    }

}
