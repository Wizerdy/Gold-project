using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBorderPosition : MonoBehaviour
{
    public Material grayScaleBorder;

    void Update()
    {
        grayScaleBorder.SetFloat("_Border", transform.position.x*-1);
    }
}
