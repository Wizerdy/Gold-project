using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetColor : MonoBehaviour
{
    public void WhiteColor(GameObject unit)
    {
        Unit unitRef = (unit.GetComponent<DragHandeler>().structType == Type.FACTORY ?
            GameManager.instance.units["WHITE"].GetComponent<Unit>() :
            GameManager.instance.units["T_WHITE"].GetComponent<Unit>());

        unit.GetComponent<Image>().color = GameManager.instance.differentsColors[0];
        unit.GetComponent<DragHandeler>().colorState = Colors.WHITE;
        unit.GetComponent<DragHandeler>().costText.text = unitRef.cost.ToString();
        unit.GetComponent<DragHandeler>().cost = unitRef.cost;
        unit.GetComponent<DragHandeler>().descText.text = unitRef.description;
        unit.GetComponent<DragHandeler>().NameText.text = unitRef.surname;
    }
}
