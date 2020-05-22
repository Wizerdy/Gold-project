﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Colors
{
    YELLOW,
    RED,
    BLUE,
    WHITE,
    GREEN,
    ORANGE,
    PURPLE,
    BLACK
}

public enum Type
{
    FACTORY,
    TURRET,
    PUMP
}

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public static GameObject itemBeingDragged;
    public GameObject michel;
    //public string unitToSpawn;

    public int cost;

    public Colors colorState;

    public Type structType;

    Vector3 startPosition;

    Transform startParent;

    public bool barren = false;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (barren == true && ShopManager.instance.Pay(cost) == true)
        {
            string name = (structType == Type.TURRET ? "T_" : "") + colorState.ToString();
            if (structType == Type.TURRET &&
                ShopManager.instance.spawnList[ShopManager.instance.currentHudId].childCount > 0)
            {
                if (colorState == ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetChild(0).GetComponent<Turret>().color)
                {
                    return;
                }
                Destroy(ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetChild(0).gameObject);
            }
                GameManager.instance.InstantiateUnit(GameManager.instance.units[name], Unit.Side.ALLY,
                    ShopManager.instance.spawnList[ShopManager.instance.currentHudId]);
        }
    }

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
        GameObject mergedColor = null;
        switch(colorState)
        {
            case Colors.YELLOW:
                switch(bernardColor)
                {
                    case Colors.RED:
                        mergedColor = ShopManager.instance.superColors[0 + term];
                        break;
                    case Colors.BLUE:
                        mergedColor = ShopManager.instance.superColors[1 + term];
                        break;
                    case Colors.ORANGE:
                    case Colors.PURPLE:
                    case Colors.GREEN:
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                    case Colors.WHITE:
                        mergedColor = ShopManager.instance.superColors[4 + term];
                        break;
                }
                break;
            case Colors.RED:
                switch (bernardColor)
                {
                    case Colors.YELLOW:
                        mergedColor = ShopManager.instance.superColors[0 + term];
                        break;
                    case Colors.BLUE:
                        mergedColor = ShopManager.instance.superColors[2 + term];
                        break;
                    case Colors.ORANGE:
                    case Colors.PURPLE:
                    case Colors.GREEN:
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                    case Colors.WHITE:
                        mergedColor = ShopManager.instance.superColors[5 + term];
                        break;
                }
                break;
            case Colors.BLUE:
                switch (bernardColor)
                {
                    case Colors.RED:
                        mergedColor = ShopManager.instance.superColors[2 + term];
                        break;
                    case Colors.YELLOW:
                        mergedColor = ShopManager.instance.superColors[1 + term];
                        break;
                    case Colors.ORANGE:
                    case Colors.PURPLE:
                    case Colors.GREEN:
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                    case Colors.WHITE:
                        mergedColor = ShopManager.instance.superColors[3 + term];
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
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                    case Colors.WHITE:
                        mergedColor = ShopManager.instance.superColors[0 + term];
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
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                    case Colors.WHITE:
                        mergedColor = ShopManager.instance.superColors[2 + term];
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
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                    case Colors.WHITE:
                        mergedColor = ShopManager.instance.superColors[1 + term];
                        break;
                }
                break;
            case Colors.BLACK:
                switch (bernardColor)
                {
                    case Colors.WHITE:
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                }
                break;
            case Colors.WHITE:
                switch (bernardColor)
                {
                    case Colors.RED:
                        mergedColor = ShopManager.instance.superColors[5 + term];
                        break;
                    case Colors.BLUE:
                        mergedColor = ShopManager.instance.superColors[3 + term];
                        break;
                    case Colors.YELLOW:
                        mergedColor = ShopManager.instance.superColors[4 + term];
                        break;
                    case Colors.PURPLE:
                        mergedColor = ShopManager.instance.superColors[2 + term];
                        break;
                    case Colors.GREEN:
                        mergedColor = ShopManager.instance.superColors[1 + term];
                        break;
                    case Colors.ORANGE:
                        mergedColor = ShopManager.instance.superColors[0 + term];
                        break;
                    case Colors.BLACK:
                        mergedColor = ShopManager.instance.superColors[6 + term];
                        break;
                }
                break;
        }

        if (mergedColor == null)
        {
            return;
        }

        Instantiate(mergedColor, bernard.transform.parent).GetComponent<DragHandeler>().barren = true;
        ShopManager.instance.moneyText[structType == Type.TURRET ? 1 : 0].text = mergedColor.GetComponent<DragHandeler>().cost.ToString();
        if (mergedColor.GetComponent<DragHandeler>().structType == Type.TURRET)
        {
            ShopManager.instance.spawnList[ShopManager.instance.currentHudId].GetComponent<CastleTurretSlot>().color = mergedColor;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (barren == false)
        {
            michel = Instantiate(michel, startParent);
            michel.GetComponent<DragHandeler>().barren = false;
        }
        Destroy(bernard);
        Destroy(this.gameObject);
    }
}
