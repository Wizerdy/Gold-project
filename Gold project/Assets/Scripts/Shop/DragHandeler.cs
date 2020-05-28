using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Colors
{
    WHITE,
    RED,
    YELLOW,
    BLUE,
    ORANGE,
    PURPLE,
    GREEN,
    BLACK
}

public enum Type
{
    FACTORY,
    TURRET,
    PUMP
}

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public GameObject michel;
    //public string unitToSpawn;

    public int cost;

    public Colors colorState;

    public Type structType;

    Vector3 startPosition;

    Transform startParent;

    public Text text;

    public bool barren = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (barren == false)
        {
            itemBeingDragged = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (barren == false)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
        else if (barren != true) {
            Instantiate(michel, startParent);
            barren = true;
        }
    }


    public void Merge(GameObject bernard)
    {
        if (bernard == null)
        {
            return;
        }
        Colors bernardColor = bernard.GetComponent<DragHandeler>().colorState;
        int term = (bernard.GetComponent<DragHandeler>().structType == Type.TURRET ? 7 : 0);
        Colors mergedColor = Colors.WHITE;
        switch (colorState)
        {
            case Colors.YELLOW:
                switch (bernardColor)
                {
                    case Colors.RED:
                        mergedColor = Colors.ORANGE;
                        break;
                    case Colors.BLUE:
                        mergedColor = Colors.GREEN;
                        break;
                    case Colors.ORANGE:
                    case Colors.PURPLE:
                    case Colors.GREEN:
                        mergedColor = Colors.BLACK;
                        break;
                    case Colors.WHITE:
                        mergedColor = Colors.YELLOW;
                        break;
                }
                break;
            case Colors.RED:
                switch (bernardColor)
                {
                    case Colors.YELLOW:
                        mergedColor = Colors.ORANGE;
                        break;
                    case Colors.BLUE:
                        mergedColor = Colors.PURPLE;
                        break;
                    case Colors.ORANGE:
                    case Colors.PURPLE:
                    case Colors.GREEN:
                        mergedColor = Colors.BLACK;
                        break;
                    case Colors.WHITE:
                        mergedColor = Colors.RED;
                        break;
                }
                break;
            case Colors.BLUE:
                switch (bernardColor)
                {
                    case Colors.RED:
                        mergedColor = Colors.PURPLE;
                        break;
                    case Colors.YELLOW:
                        mergedColor = Colors.GREEN;
                        break;
                    case Colors.ORANGE:
                    case Colors.PURPLE:
                    case Colors.GREEN:
                        mergedColor = Colors.BLACK;
                        break;
                    case Colors.WHITE:
                        mergedColor = Colors.BLUE;
                        break;
                }
                break;
            case Colors.ORANGE:
                switch (bernardColor)
                {
                    case Colors.RED:
                    case Colors.BLUE:
                    case Colors.YELLOW:
                    case Colors.PURPLE:
                    case Colors.GREEN:
                        mergedColor = Colors.BLACK;
                        break;
                    case Colors.WHITE:
                        mergedColor = Colors.ORANGE;
                        break;
                }
                break;
            case Colors.PURPLE:
                switch (bernardColor)
                {
                    case Colors.RED:
                    case Colors.BLUE:
                    case Colors.YELLOW:
                    case Colors.ORANGE:
                    case Colors.GREEN:
                        mergedColor = Colors.BLACK;
                        break;
                    case Colors.WHITE:
                        mergedColor = Colors.PURPLE;
                        break;
                }
                break;
            case Colors.GREEN:
                switch (bernardColor)
                {
                    case Colors.RED:
                    case Colors.BLUE:
                    case Colors.YELLOW:
                    case Colors.PURPLE:
                    case Colors.ORANGE:
                        mergedColor = Colors.BLACK;
                        break;
                    case Colors.WHITE:
                        mergedColor = Colors.GREEN;
                        break;
                }
                break;
            case Colors.BLACK:
                switch (bernardColor)
                {
                    case Colors.WHITE:
                        mergedColor = Colors.BLACK;
                        break;
                }
                break;
            case Colors.WHITE:
                switch (bernardColor)
                {
                    case Colors.RED:
                        mergedColor = Colors.RED;
                        break;
                    case Colors.BLUE:
                        mergedColor = Colors.BLUE;
                        break;
                    case Colors.YELLOW:
                        mergedColor = Colors.YELLOW;
                        break;
                    case Colors.PURPLE:
                        mergedColor = Colors.PURPLE;
                        break;
                    case Colors.GREEN:
                        mergedColor = Colors.GREEN;
                        break;
                    case Colors.ORANGE:
                        mergedColor = Colors.ORANGE;
                        break;
                    case Colors.BLACK:
                        mergedColor = Colors.BLACK;
                        break;
                }
                break;
        }

        bernard.GetComponent<Image>().color = GameManager.instance.differentsColors[(int)mergedColor];

        //Instantiate(mergedColor, bernard.transform.parent).GetComponent<DragHandeler>().barren = true;
        //ShopManager.instance.moneyText[structType == Type.TURRET ? 1 : 0].text = mergedColor.GetComponent<DragHandeler>().cost.ToString();
        /*if (mergedColor.GetComponent<DragHandeler>().structType == Type.TURRET)
        {
            ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetComponent<CastleTurretSlot>().color = mergedColor;
        }*/
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (barren == false)
        {
            michel = Instantiate(michel, startParent);
            michel.GetComponent<DragHandeler>().barren = false;
        }

        string name = (bernard.GetComponent<DragHandeler>().structType == Type.TURRET ? "T_" : "") + mergedColor.ToString();

        bernard.GetComponent<DragHandeler>().colorState = mergedColor;

        bernard.GetComponent<DragHandeler>().cost = GameManager.instance.units[name].GetComponent<Unit>().cost;

        bernard.GetComponent<DragHandeler>().text.text = GameManager.instance.units[name].GetComponent<Unit>().cost.ToString();
        Destroy(this.gameObject);
    }
}
