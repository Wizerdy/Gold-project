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
            ShopManager.instance.ActiveCanvas(0);
            ShopManager.instance.currentHudId = hudId;
        }
    }
}
