using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretButton : MonoBehaviour
{
    public void TurretBuy(DragHandeler Turret)
    {
        if (ShopManager.instance.spawnList[ShopManager.instance.currentHudId].childCount > 0)
        {
            Destroy(ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetChild(0).gameObject);
        }
            string name = (Turret.structType == Type.TURRET ? "T_" : "") + Turret.colorState.ToString();
            GameManager.instance.InstantiateUnit(GameManager.instance.units[name], Unit.Side.ALLY, ShopManager.instance.spawnList[ShopManager.instance.currentHudId]);
    }
}
