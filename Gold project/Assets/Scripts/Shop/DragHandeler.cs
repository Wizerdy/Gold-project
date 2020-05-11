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
    WHITE,
    GREEN,
    ORANGE,
    PURPLE,
    BLACK
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
        Debug.Log(bernard);
        if (bernard == null)
        {
            return;
        }
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
                    mergedColor = ShopManager.instance.superColors[4];
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
                    mergedColor = ShopManager.instance.superColors[5];
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
                    mergedColor = ShopManager.instance.superColors[3];
                }
                break;
        }

        if (mergedColor == null)
        {
            return;
        }

        Instantiate(mergedColor, bernard.transform.parent).GetComponent<DragHandeler>().barren = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        michel = Instantiate(michel, startParent);
        michel.GetComponent<DragHandeler>().barren = false;
        Destroy(bernard);
        Destroy(this.gameObject);
    }
}
