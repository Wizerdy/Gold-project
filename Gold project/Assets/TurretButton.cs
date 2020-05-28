using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretButton : MonoBehaviour
{
    public void TurretBuy(DragHandeler Turret)
    {
        if (ShopManager.instance.currentHudId != 0 && ShopManager.instance.currentHudId != 5 && ShopManager.instance.currentHudId != 7 && ShopManager.instance.currentHudId != 9)
        {
            if (ShopManager.instance.spawnList[ShopManager.instance.currentHudId].childCount > 0)
            {
                Destroy(ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetChild(0).gameObject);
            }

            if (ShopManager.instance.Pay(Turret.cost) == true)
            {
                string name = (Turret.structType == Type.TURRET ? "T_" : "") + Turret.colorState.ToString();
                GameObject insta = GameManager.instance.InstantiateUnit(GameManager.instance.units[name], Unit.Side.ALLY, ShopManager.instance.spawnList[ShopManager.instance.currentHudId]);

                if (ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetComponent<OrderInLayer>() != null)
                {
                    int add = ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetComponent<OrderInLayer>().orderInLayer;

                    SpriteRenderer[] sprRenders = insta.GetComponentsInChildren<SpriteRenderer>();
                    for (int i = 0; i < sprRenders.Length; i++)
                    {
                        sprRenders[i].sortingOrder += add;
                    }
                }
            }
        }
    }
}
