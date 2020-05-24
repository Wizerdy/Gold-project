using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTurretSlot : MonoBehaviour
{
    public int hudId;
    public GameObject color;

    private void OnMouseDown()
    {
        ShopManager.instance.ActiveCanvas(1);
        ShopManager.instance.currentHudId = hudId;
        //Debug.Log(ShopManager.instance.canvasList[1].transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0));
        if (transform.childCount > 0)
        {
            if (transform.GetChild(0).GetComponent<Turret>().color != ShopManager.instance.canvasList[1].transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<DragHandeler>().colorState)
            {
                Destroy(ShopManager.instance.canvasList[1].transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject);
                Instantiate(color, ShopManager.instance.canvasList[1].transform.GetChild(0).GetChild(1).GetChild(1)).GetComponent<DragHandeler>().barren = true;
            }
        }
    }

    public void SetHudId(int id)
    {
        hudId = id;
    }
}
