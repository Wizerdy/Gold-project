using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject[] canvasList;

    public int health;

    public int currentHudId;
    public int money;

    public GameObject[] Pumps;

    public Text moneyText;
    public Text healthText;

    public Transform[] spawnList;

    public bool Pay(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            moneyText.text = money.ToString();
            return true;
        } else
        {
            return false;
        }
    }

    public void SetHudId(int id)
    {
        currentHudId = id;
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
        moneyText.text = money.ToString();
        healthText.text = health.ToString();

    }

    public void Gain(int amount)
    {
        money += amount;
    }
}
