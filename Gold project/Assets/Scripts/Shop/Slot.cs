using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public bool isInventorySlot;

    public GameObject item {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragHandeler.itemBeingDragged.transform.SetParent(transform);
        }
        else if (item.GetComponent<DragHandeler>().barren == true)
        {
            DragHandeler.itemBeingDragged.GetComponent<DragHandeler>().Merge(item);
        }
    }
}
