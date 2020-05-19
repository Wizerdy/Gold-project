﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThatAss : MonoBehaviour
{
    public Transform target;
    public float speed;

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.position, speed*Time.deltaTime);
    }
}
