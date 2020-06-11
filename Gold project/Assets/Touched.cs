using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touched : MonoBehaviour
{
    public int hudId;
    public bool active;

    private void OnMouseDown()
    {
        if (active)
        {
            ShopManager.instance.SetHudId(hudId);
            ShopManager.instance.ActiveCanvas(hudId == 0 ? 0 : hudId == 5 ? 1 : hudId == 7 ? 2 : hudId == 9 ? 3 : 0);
            ShopManager.instance.palette.SetActive(true);
        }
    }
}
