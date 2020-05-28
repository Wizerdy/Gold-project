using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Settings")]
    [Range(0f, 1f)] public float slimeMinSize;
    [Range(0f, 1f)] public float slimeRefund;

    [Header("Minimap")]
    [SerializeField] private Transform minimapParent;
    [SerializeField] private GameObject castle;
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject unit;

    [SerializeField] private Color allyColor;
    [SerializeField] private Color neutralColor;
    [SerializeField] private Color enemyColor;

    [Header("Parents")]
    public Transform allyParent;
    public Transform enemyParent;

    [Header("Layers")]
    public LayerMask unitLayer;
    public LayerMask floorLayer;

    [Header("GameObject")]
    public GameObject splash;
    public GameObject deathParticle;
    public GameObject explosion;

    [Header("Array")]
    public Color[] differentsColors;

    [Header("Ohter")]
    public Material grayScaleBorder;

    [HideInInspector] public Dictionary<string, GameObject> units;
    [HideInInspector] public List<GameObject> allies;
    private RaycastHit2D[] mnpHits;

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

        for (int i = 0; i < hits.Length; i++)
            if(hits[i].GetComponent<Unit>().side == Unit.Side.ALLY)
                allies.Add(hits[i].gameObject);
    }

    private void Update()
    {
        Minimap();

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

    public GameObject InstantiateUnit(GameObject unit, Transform parent, Transform target)
    {
        GameObject insta = Instantiate(unit, parent);
        insta.GetComponent<Pawn>().target = target;

        return insta;
    }

    public GameObject InstantiateUnit(GameObject unit, Unit.Side side, Vector2 pos)
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

        return insta;
    }

    public GameObject InstantiateUnit(GameObject unit, Unit.Side side, Transform parent)
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

        return insta;
    }

    public void SpawnSplash(Vector2 pos, Color color)
    {
        GameObject insta = Instantiate(splash, pos, Quaternion.identity);
        float size = UnityEngine.Random.Range(0.1f, 1);
        insta.transform.localScale = new Vector3(size, size, 1);
        insta.GetComponent<SpriteRenderer>().color = color;
        GameObject insta2 = Instantiate(deathParticle, pos, deathParticle.transform.rotation);
        insta2.GetComponent<ParticleSystem>().startColor = color;

    }

    public static bool InsideLayer(int layer, LayerMask mask)
    {
        if (mask == (mask | (1 << layer)))
            return true;

        return false;
    }

    private void Minimap()
    {
        
        if(minimapParent != null)
        {
            List<GameObject> childs = new List<GameObject>();
            for (int i = 0; i < minimapParent.childCount; i++)
                childs.Add(minimapParent.GetChild(i).gameObject);

            float dist = enemyParent.position.x - allyParent.position.x;
            mnpHits = Physics2D.RaycastAll(allyParent.position, enemyParent.position - allyParent.position, dist, unitLayer);
            for (int i = 0; i < mnpHits.Length; i++)
            {
                Unit hitted = mnpHits[i].collider.gameObject.GetComponent<Unit>();
                GameObject needed = null;
                switch(hitted.type)
                {
                    case Unit.Type.PAWN:
                        needed = unit;
                        break;
                    case Unit.Type.STRUCTURE:
                        needed = tower;
                        break;
                }

                GameObject insta = SearchChildName(minimapParent, needed.name);

                if (insta == null)
                {
                    insta = Instantiate(needed, minimapParent);
                    insta.name = needed.name;
                }
                else
                {
                    childs.Remove(insta);
                    insta.SetActive(true);
                }

                insta.GetComponent<RectTransform>().anchoredPosition = new Vector2(mnpHits[i].distance / dist * minimapParent.GetComponent<RectTransform>().rect.width, 0);
                Color color = Color.white;
                switch (hitted.side)
                {
                    case Unit.Side.NEUTRAL:
                        color = neutralColor;
                        break;
                    case Unit.Side.ALLY:
                        color = allyColor;
                        break;
                    case Unit.Side.ENEMY:
                        color = enemyColor;
                        break;
                }
                insta.GetComponent<Image>().color = color;
            }

            for (int i = 0; i < childs.Count; i++)
                childs[i].gameObject.SetActive(false);

            for (int i = 0; i < minimapParent.childCount; i++)
                if (minimapParent.GetChild(i).name == unit.name)
                    minimapParent.GetChild(i).SetAsLastSibling();
        }
    }

    private GameObject SearchChildName(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
            if(!parent.GetChild(i).gameObject.activeSelf && parent.GetChild(i).name == name)
                return parent.GetChild(i).gameObject;

        return null;
    }
}
