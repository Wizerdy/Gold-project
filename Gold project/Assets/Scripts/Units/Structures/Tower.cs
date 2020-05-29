using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tower : Structure
{
    [HideInInspector] public Side lastDamageSide;

    public Transform spawnPoint;
    public List<Transform> turrets;
    public List<Transform> pumps;

    [Header("Sprites")]
    [SerializeField] private List<SpriteRenderer> toColor;

    [Header("OnDead")]
    [SerializeField] private UnityEvent action;

    protected override void Start()
    {
        base.Start();
        Touched touch = GetComponent<Touched>();
        if (side == Side.ALLY && touch != null)
            touch.active = true;

        ChangeColor(GameManager.instance.neutralColor);
    }

    public override void LoseHealth(int amount)
    {
        base.LoseHealth(amount);
        ShopManager.instance.RefreshHUD();
    }

    protected override void Die()
    {
        //for (int i = 1; i < transform.childCount; i++)
            //Destroy(transform.GetChild(i).gameObject);

        side = lastDamageSide;

        for (int i = 0; i < turrets.Count; i++)
            for (int j = 0; j < turrets[i].childCount; j++)
            {
                Destroy(turrets[i].GetChild(j).gameObject);
                if(side == Side.ENEMY)
                    GameManager.instance.iA.turretCount--;
            }

        for (int i = 0; i < pumps.Count; i++)
        {
            pumps[i].gameObject.SetActive(false);
            pumps[i].GetComponent<Pump>().enabled = true;
            if (side == Side.ENEMY)
                GameManager.instance.iA.pumpCount--;
        }

        Touched touch = GetComponent<Touched>();

        Color coloration = coloration = GameManager.instance.neutralColor;

        switch (side)
        {
            case Side.ALLY:
                if (touch != null)
                    touch.active = true;
                coloration = GameManager.instance.allyColor;
                GameManager.instance.allies.Add(gameObject);
                break;
            case Side.ENEMY:
                if (touch != null)
                    touch.active = false;
                coloration = GameManager.instance.enemyColor;
                break;
            case Side.NEUTRAL:
                if (touch != null)
                    touch.active = false;
                coloration = GameManager.instance.neutralColor;
                break;
        }

        ChangeColor(coloration);

        curHealth = maxHealth;

        if(action != null)
        {
            action.Invoke();
        }
    }

    private void ChangeColor(Color color)
    {
        for (int i = 0; i < toColor.Count; i++)
            toColor[i].color = color;
    }
}
