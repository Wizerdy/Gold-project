using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyScaleMouse : MonoBehaviour
{
    public Material grayScaleBorder;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            grayScaleBorder.SetFloat("_Border", Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x * -1);
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x);
        }

    }
}
