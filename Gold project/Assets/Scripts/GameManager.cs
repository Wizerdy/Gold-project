using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Settings")]
    [Range(0f, 1f)] public float slimeMinSize;
    [Range(0f, 1f)] public float slimeRefund;

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

    [Header("Ohter")]
    public Material grayScaleBorder;

    [HideInInspector] public Dictionary<string, GameObject> units;
     public List<GameObject> allies;

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

    private void Start()
    {
        allies = new List<GameObject>();
        Collider2D[] hits = Physics2D.OverlapBoxAll(Vector2.zero, new Vector2(300, 50), 0, unitLayer);
        Debug.Log(hits.Length);

        for (int i = 0; i < hits.Length; i++)
            if(hits[i].GetComponent<Unit>().side == Unit.Side.ALLY)
                allies.Add(hits[i].gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            InstantiateUnit(units["Knight"], Unit.Side.ALLY, allyParent.position);

        else if (Input.GetKeyDown(KeyCode.Z))
            InstantiateUnit(units["WHITE"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.E))
            InstantiateUnit(units["REDS"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.R))
            InstantiateUnit(units["YELLOW"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.T))
            InstantiateUnit(units["BLUE"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.Y))
            InstantiateUnit(units["ORANGE"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.U))
            InstantiateUnit(units["GREEN"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.I))
            InstantiateUnit(units["PURPLE"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.O))
            InstantiateUnit(units["BLACK"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.P))
            InstantiateUnit(units["LITTLEPURPLE"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.S))
            InstantiateUnit(units["WHITE"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.D))
            InstantiateUnit(units["RED"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.F))
            InstantiateUnit(units["YELLOW"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.G))
            InstantiateUnit(units["BLUE"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.H))
            InstantiateUnit(units["ORANGE"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.J))
            InstantiateUnit(units["GREEN"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.K))
            InstantiateUnit(units["PURPLE"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.L))
            InstantiateUnit(units["BLACK"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKeyDown(KeyCode.M))
            InstantiateUnit(units["LITTLEPURPLE"], Unit.Side.ENEMY, enemyParent.position);
        else if (Input.GetKey(KeyCode.Q))
            Destroy(GameObject.FindWithTag("Slime"));


        int index = 0;
        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i] == null || allies[i].GetComponent<Unit>().side != Unit.Side.ALLY)
                allies.RemoveAt(i);

            if (index < allies.Count && i < allies.Count &&
                allies[index].transform.position.x < allies[i].transform.position.x
            )
                index = i;
        }

        if (index < allies.Count)
            grayScaleBorder.SetFloat("_Border", allies[index].transform.position.x * -1);
        else if(allies.Count <= 0)
            grayScaleBorder.SetFloat("_Border", 20);
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
            insta.GetComponent<Pawn>().target = (side == Unit.Side.ALLY ? enemyParent : allyParent);

        if (side == Unit.Side.ALLY)
            allies.Add(insta);
    }

    public void InstantiateUnit(GameObject unit, Unit.Side side, Transform parent)
    {
        GameObject insta = Instantiate(unit, parent);
        insta.transform.position = parent.position;
        //insta.layer = (side == Unit.Side.ALLY ? allyParent : enemyParent).gameObject.layer;
        insta.transform.localEulerAngles = new Vector3(insta.transform.rotation.x, insta.transform.rotation.y + 180 * (side == Unit.Side.ALLY ? 0 : 1), insta.transform.rotation.z);
        insta.GetComponent<Unit>().side = side;

        if (insta.GetComponent<Unit>().type == Unit.Type.PAWN)
            insta.GetComponent<Pawn>().target = (side == Unit.Side.ALLY ? enemyParent : allyParent);

        if (side == Unit.Side.ALLY)
            allies.Add(insta);
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
