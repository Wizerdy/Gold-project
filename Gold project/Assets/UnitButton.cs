using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public void BuyUnit(DragHandeler unit)
    {
        if (ShopManager.instance.Pay(unit.cost) == true)
        {
            string name = (unit.structType == Type.TURRET ? "T_" : "") + unit.colorState.ToString();
            GameObject unitSpawned = GameManager.instance.InstantiateUnit(GameManager.instance.units[name], Unit.Side.ALLY, ShopManager.instance.spawnList[ShopManager.instance.currentHudId]);
            unitSpawned.transform.localScale = new Vector3(Mathf.Sqrt(unitSpawned.GetComponent<Knight>().maxHealth / 50) / 2, Mathf.Sqrt(unitSpawned.GetComponent<Knight>().maxHealth / 50) / 2, 1);
        }
    }
}
