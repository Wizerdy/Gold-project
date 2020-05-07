using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Colors
{
    YELLOW,
    RED,
    BLUE,
    WHITE
}

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public Colors colorState;
    Vector3 startPosition;
    Transform startParent;
    bool barren = true;
    public GameObject michel;

    public void OnBeginDrag(PointerEventData eventData)
    {

        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
        else if (barren != false) {
            Instantiate(michel, startParent);
            barren = false;
        }
    }

    public void Merge(GameObject bernard)
    {
        Colors bernardColor = bernard.GetComponent<DragHandeler>().colorState;
        GameObject mergedColor = null;
        switch(colorState)
        {
            case Colors.YELLOW:
                if (bernardColor == Colors.RED)
                {
                    mergedColor = ShopManager.instance.superColors[0];
                } else if (bernardColor == Colors.BLUE)
                {
                    mergedColor = ShopManager.instance.superColors[1];

                }
                else if (bernardColor == Colors.WHITE)
                {
                    
                }
                break;
            case Colors.RED:
                if (bernardColor == Colors.YELLOW)
                {
                    mergedColor = ShopManager.instance.superColors[0];

                }
                else if (bernardColor == Colors.BLUE)
                {
                    mergedColor = ShopManager.instance.superColors[2];

                }
                else if (bernardColor == Colors.WHITE)
                {

                }
                break;
            case Colors.BLUE:
                if (bernardColor == Colors.RED)
                {
                    mergedColor = ShopManager.instance.superColors[2];

                }
                else if (bernardColor == Colors.YELLOW)
                {
                    mergedColor = ShopManager.instance.superColors[1];

                }
                else if (bernardColor == Colors.WHITE)
                {

                }
                break;
            default:
                mergedColor = ShopManager.instance.superColors[0];
                break;
        }
        Instantiate(mergedColor, bernard.transform.parent).GetComponent<DragHandeler>().barren = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Instantiate(michel, startParent);
        Destroy(bernard);
        Destroy(this.gameObject);
    }
}
