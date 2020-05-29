using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetColor : MonoBehaviour
{
    public void WhiteColor(GameObject unit)
    {
        unit.GetComponent<Image>().color = GameManager.instance.differentsColors[0];
        unit.GetComponent<DragHandeler>().colorState = Colors.WHITE;
        unit.GetComponent<DragHandeler>().text.text = "5";
        unit.GetComponent<DragHandeler>().cost = 5;
    }
}
