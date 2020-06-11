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

    public Tower health;

    public int currentHudId;
    public int money;

    public GameObject[] Pumps;

    public Text moneyText;
    public Text healthText;

    public Transform[] spawnList;

    public GameObject palette;

    public bool Pay(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            RefreshHUD();
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
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        moneyText.text = money.ToString();
        healthText.text = health.maxHealth.ToString();
    }

    private void Update()
    {
        healthText.text = health.curHealth.ToString();
    }

    public void RefreshHUD()
    {
        moneyText.text = money.ToString();
        healthText.text = health.curHealth.ToString();
    }

    public void Gain(int amount)
    {
        money += amount;
        RefreshHUD();
    }
}
