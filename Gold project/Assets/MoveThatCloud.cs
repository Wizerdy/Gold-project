using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThatCloud : MonoBehaviour
{
    private MoveThatAss parent;

    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<MoveThatAss>();
    }

    void Update()
    {

        if (transform.position.x > parent.boundary.max.x)
        {
            transform.position = new Vector2(parent.boundary.min.x, transform.position.y);
        }

        transform.position += Vector3.right * (parent.speed * Time.deltaTime);
    }
}
