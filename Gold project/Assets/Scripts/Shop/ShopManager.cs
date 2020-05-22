using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject[] canvasList;

    public int currentHudId;
    public int money;

    public TextMeshProUGUI[] moneyText;

    public Transform[] spawnList;

    public bool Pay(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            return true;
        } else
        {
            return false;
        }
    }

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
