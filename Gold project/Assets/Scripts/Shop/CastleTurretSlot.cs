using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTurretSlot : MonoBehaviour
{
    public int hudId;
    public GameObject color;

    private void OnMouseDown()
    {
        ShopManager.instance.currentHudId = hudId;
        //Debug.Log(ShopManager.instance.canvasList[1].transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0));
    }
}
