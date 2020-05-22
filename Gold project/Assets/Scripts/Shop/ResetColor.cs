using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetColor : MonoBehaviour
{
    public GameObject white;

    public void WhiteColor(GameObject slot)
    {
        Destroy(slot.transform.GetChild(0).gameObject);
        Instantiate(white, slot.transform).GetComponent<DragHandeler>().barren = true;
        ShopManager.instance.moneyText[white.GetComponent<DragHandeler>().structType == Type.TURRET ? 1 : 0].text = white.GetComponent<DragHandeler>().cost.ToString();
    }
}
