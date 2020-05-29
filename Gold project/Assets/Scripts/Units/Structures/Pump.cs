using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : Structure
{
    protected override void Start()
    {
        StartCoroutine("Gain");
    }

    private IEnumerator Gain()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.instance.pumpTime);
            ShopManager.instance.Gain(GameManager.instance.pumpAmount);
        }
    }
}
