using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDisabler : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData) // 3
    {
        Camera.main.GetComponent<CameraScrolling>().active = true;
        ShopManager.instance.ActiveCanvas(-1);
        ShopManager.instance.palette.SetActive(false);
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUp()
    {
        Camera.main.GetComponent<CameraScrolling>().active = false;
    }
}
