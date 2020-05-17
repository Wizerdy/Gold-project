using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
    public GameObject UI;

    private void OnMouseDown()
    {
        if (UI.activeSelf)
        {
            UI.SetActive(false);
        }
        else
        {
            UI.SetActive(true);
        }
    }
}
