using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Parents")]
    public Transform allyParent;
    public Transform enemyParent;

    [HideInInspector] public Dictionary<string, GameObject> units;

    private void Awake()
    {
        instance = this;

        units = new Dictionary<string, GameObject>();
        GameObject[] obj = Resources.LoadAll<GameObject>("Units");
        for (int i = 0; i < obj.Length; i++)
        {
            units.Add(obj[i].name, obj[i]);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            InstantiateUnit(units["Knight"], Unit.Side.ALLY);
         else if(Input.GetKeyDown(KeyCode.E))
            InstantiateUnit(units["Knight"], Unit.Side.ENEMY);
    }

    public void InstantiateUnit(GameObject unit, Transform parent, Transform target)
    {
        GameObject insta = Instantiate(unit, parent);
        insta.GetComponent<Pawn>().target = target;
    }

    public void InstantiateUnit(GameObject unit, Unit.Side side)
    {
        GameObject insta = Instantiate(unit, (side == Unit.Side.ALLY ? allyParent : enemyParent) );
        insta.GetComponent<Unit>().side = side;

        if(insta.GetComponent<Unit>().type == Unit.Type.PAWN)
            insta.GetComponent<Pawn>().target = (side == Unit.Side.ALLY ? enemyParent : allyParent);
    }
}
