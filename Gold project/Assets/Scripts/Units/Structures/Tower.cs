﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
    public int hudId;

    private void OnMouseDown()
    {
        ShopManager.instance.ActiveCanvas(0);
        ShopManager.instance.currentHudId = hudId;
    }
}
