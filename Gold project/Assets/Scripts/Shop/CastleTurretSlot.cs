using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTurretSlot : MonoBehaviour
{
    public int hudId;

    private void OnMouseDown()
    {
        ShopManager.instance.ActiveCanvas(1);
        ShopManager.instance.currentHudId = hudId;
    }
}
