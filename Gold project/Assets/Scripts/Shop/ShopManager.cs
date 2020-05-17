using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject[] canvasList;

    public int currentHudId;

    public Transform[] spawnList;

    public void ActiveCanvas(int number)
    {
        for (int i = 0; i < canvasList.Length; i++)
        {
            canvasList[i].SetActive(false);
        }
        if (number >= 0)
        {
            canvasList[number].SetActive(true);
        }
    }

    private void Awake()
    {
        instance = this;        
    }

    public GameObject[] superColors;
}
