using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public void AddInventorySlot(GameObject slot)
    {
        if (transform.parent.childCount < 9)
        {
            slot = Instantiate(slot, transform.parent);
            slot.transform.SetSiblingIndex(transform.parent.childCount - 2);
            if (transform.parent.childCount >= 9) {
                gameObject.SetActive(false);
            }
        }
    }
}
