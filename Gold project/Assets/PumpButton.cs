using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PumpButton : MonoBehaviour
{
    public int number;
    public Text textToChange;

    public void ActivePump()
    {
        if (ShopManager.instance.Pay(ShopManager.instance.Pumps[number].GetComponent<Unit>().cost))
        {
            ShopManager.instance.Pumps[number].SetActive(true);
            if (number < 3)
            {
                number++;
            }
            textToChange.text = ShopManager.instance.Pumps[number].GetComponent<Unit>().cost.ToString();
        }
    }
}
