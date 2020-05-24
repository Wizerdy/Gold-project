using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public void BuyUnit(DragHandeler unit)
    {
        string name = (unit.structType == Type.TURRET ? "T_" : "") + unit.colorState.ToString();
        GameManager.instance.InstantiateUnit(GameManager.instance.units[name], Unit.Side.ALLY, ShopManager.instance.spawnList[ShopManager.instance.currentHudId]);
    }
}
