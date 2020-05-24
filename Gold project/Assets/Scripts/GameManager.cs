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

    [Header("Layers")]
    public LayerMask unitLayer;
    public LayerMask floorLayer;

    [Header("GameObject")]
    public GameObject splash;
    public GameObject explosion;

    [Header("Array")]
    public Color[] differentsColors;

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
        if (Input.GetKeyDown(KeyCode.A))
            InstantiateUnit(units["Knight"], Unit.Side.ALLY, allyParent.position);

        else if (Input.GetKeyDown(KeyCode.Z))
            InstantiateUnit(units["WHITESLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.E))
            InstantiateUnit(units["REDSLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.R))
            InstantiateUnit(units["YELLOWSLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.T))
            InstantiateUnit(units["BLUESLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.Y))
            InstantiateUnit(units["ORANGESLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.U))
            InstantiateUnit(units["GREENSLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.I))
            InstantiateUnit(units["PURPLESLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.O))
            InstantiateUnit(units["BLACKSLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.P))
            InstantiateUnit(units["LITTLEPURPLESLIME"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.S))
            InstantiateUnit(units["WHITESLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.D))
            InstantiateUnit(units["REDSLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.F))
            InstantiateUnit(units["YELLOWSLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.G))
            InstantiateUnit(units["BLUESLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.H))
            InstantiateUnit(units["ORANGESLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.J))
            InstantiateUnit(units["GREENSLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.K))
            InstantiateUnit(units["PURPLESLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.L))
            InstantiateUnit(units["BLACKSLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.M))
            InstantiateUnit(units["LITTLEPURPLESLIME"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKey(KeyCode.Q))
            Destroy(GameObject.FindWithTag("Slime"));



    }

    public void InstantiateUnit(GameObject unit, Transform parent, Transform target)
    {
        GameObject insta = Instantiate(unit, parent);
        insta.GetComponent<Pawn>().target = target;
    }

    public void InstantiateUnit(GameObject unit, Unit.Side side, Vector2 pos)
    {
        Debug.Log(pos);
        GameObject insta = Instantiate(unit, (side == Unit.Side.ALLY ? allyParent : enemyParent) );
        insta.transform.position = pos;
        //insta.layer = (side == Unit.Side.ALLY ? allyParent : enemyParent).gameObject.layer;
        insta.transform.localEulerAngles = new Vector3(insta.transform.rotation.x, insta.transform.rotation.y + 180 * (side == Unit.Side.ALLY ? 0 : 1), insta.transform.rotation.z);
        insta.GetComponent<Unit>().side = side;

        if (insta.GetComponent<Unit>().type == Unit.Type.PAWN)
        {
            insta.GetComponent<Pawn>().target = (side == Unit.Side.ALLY ? enemyParent : allyParent);
        }
    }

    public void InstantiateUnit(GameObject unit, Unit.Side side, Transform parent)
    {
        GameObject insta = Instantiate(unit, parent);
        insta.transform.position = parent.position;
        //insta.layer = (side == Unit.Side.ALLY ? allyParent : enemyParent).gameObject.layer;
        insta.transform.localEulerAngles = new Vector3(insta.transform.rotation.x, insta.transform.rotation.y + 180 * (side == Unit.Side.ALLY ? 0 : 1), insta.transform.rotation.z);
        insta.GetComponent<Unit>().side = side;

        if (insta.GetComponent<Unit>().type == Unit.Type.PAWN)
        {
            insta.GetComponent<Pawn>().target = (side == Unit.Side.ALLY ? enemyParent : allyParent);
        }
    }

    public void SpawnSplash(Vector2 pos)
    {
        Instantiate(splash, pos, Quaternion.identity);
    }

    public static bool InsideLayer(int layer, LayerMask mask)
    {
        if (mask == (mask | (1 << layer)))
            return true;

        return false;
    }
}
