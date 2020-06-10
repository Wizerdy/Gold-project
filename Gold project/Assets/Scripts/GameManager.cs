using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Settings")]
    [Range(0f, 1f)] public float slimeMinSize;
    [Range(0f, 1f)] public float slimeRefund;
    public float dotSpeed;

    public int passivePump;
    public float pumpTime;
    public int pumpCost;
    public int pumpAmount;

    [Header("Minimap")]
    [SerializeField] private Transform minimapParent;
    [SerializeField] private GameObject castle;
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject unit;

    public Color allyColor;
    public Color neutralColor;
    public Color enemyColor;

    [Header("Parents")]
    public Transform allyParent;
    public Transform enemyParent;
    public Transform splashParent;

    [Header("Layers")]
    public LayerMask unitLayer;
    public LayerMask floorLayer;
    public LayerMask splashLayer;

    [Header("GameObject")]
    public GameObject splashParticles;
    public GameObject burnParticles;
    public GameObject regenParticles;
    public GameObject splash;
    public GameObject deathParticle;
    public GameObject explosion;

    [Header("Array")]
    public Color[] differentsColors;

    [Header("Ohter")]
    public Material grayScaleBorder;
    public IAController iA;

    public GameObject victoryScreen;
    public GameObject defeatScreen;

    [HideInInspector] public Dictionary<string, GameObject> units;
    [HideInInspector] public List<GameObject> allies;

    public bool particleEnabled;
    private RaycastHit2D[] mnpHits;

    private int slimeOiL;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);

        units = new Dictionary<string, GameObject>();
        GameObject[] obj = Resources.LoadAll<GameObject>("Units");
        for (int i = 0; i < obj.Length; i++)
        {
            units.Add(obj[i].name, obj[i]);
        }

        Time.timeScale = 1;
        slimeOiL = 0;
    }

    private void Start()
    {
        allies = new List<GameObject>();
        Collider2D[] hits = Physics2D.OverlapBoxAll(Vector2.zero, new Vector2(300, 50), 0, unitLayer);

        if (hits != null)
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log(" ----- " + hits[i].name);
                if (hits[i].GetComponent<Unit>().side == Unit.Side.ALLY)
                    allies.Add(hits[i].gameObject);
            }

        StartCoroutine(Gain());
    }

    private void Update()
    {
        Minimap();

        if (Input.GetKeyDown(KeyCode.A))
            Defeat();
        //InstantiateUnit(units["Knight"], Unit.Side.ALLY, allyParent.position);

        else if (Input.GetKeyDown(KeyCode.Z))
            InstantiateUnit(units["WHITE"], Unit.Side.ALLY, allyParent.position);
        else if (Input.GetKeyDown(KeyCode.E))
            InstantiateUnit(units["RED"], Unit.Side.ALLY, allyParent.position);
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

    //public GameObject InstantiateUnit(GameObject unit, Transform parent, Transform target)
    //{
    //    GameObject insta = Instantiate(unit, parent);
    //    insta.GetComponent<Pawn>().target = target;

    //    return insta;
    //}

    public GameObject InstantiateUnit(GameObject unit, Unit.Side side, Transform parent)
    {
        GameObject insta = Instantiate(unit, parent);
        insta.transform.position = parent.position;
        //insta.layer = (side == Unit.Side.ALLY ? allyParent : enemyParent).gameObject.layer;
        insta.transform.localEulerAngles = new Vector3(insta.transform.rotation.x, insta.transform.rotation.y + 180 * (side == Unit.Side.ALLY ? 0 : 1), insta.transform.rotation.z);
        insta.GetComponent<Unit>().side = side;

        if (insta.GetComponent<Unit>().type == Unit.Type.PAWN)
        {
            insta.GetComponent<Pawn>().target = (side == Unit.Side.ALLY ? enemyParent : allyParent);
            Tools.AddOiL(insta, slimeOiL);
            slimeOiL = (slimeOiL + 5) % 100;
        }

        if (side == Unit.Side.ALLY)
            allies.Add(insta);

        return insta;
    }

    public GameObject InstantiateUnit(GameObject unit, Unit.Side side, Vector2 pos)
    {

        GameObject insta = InstantiateUnit(unit, side, (side == Unit.Side.ALLY ? allyParent : enemyParent));
        insta.transform.position = pos;

        return insta;
    }

    public GameObject SpawnSplash(Vector2 pos, Color color)
    {
        GameObject insta = Instantiate(splash, pos, Quaternion.identity);
        //float size = UnityEngine.Random.Range(0.1f, 1);
        insta.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        insta.GetComponent<SpriteRenderer>().color = color;
        insta.transform.parent = splashParent;
        insta.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
        //GameObject insta2 = Instantiate(deathParticle, pos, deathParticle.transform.rotation);
        //insta2.GetComponent<ParticleSystem>().startColor = color;
        return insta;
    }

    public GameObject SpawnSplash(Vector2 pos, Colors color)
    {
        GameObject insta = SpawnSplash(pos, differentsColors[(int)color]);
        insta.GetComponent<SpriteRenderer>().sortingOrder = (-170 + (int)color);
        return insta;
    }

    public void SpawnDamageParticles(int damage, Colors color, Vector3 position, Vector3 angle)
    {
        if (particleEnabled)
        {
            GameObject insta = Instantiate(splashParticles, position, Quaternion.identity);
            insta.transform.eulerAngles = angle;
            //int maxDamage = 20;
            //insta.GetComponent<SplashManagerInside>().SetParticleSystem(
            //   damage / 20 * maxDamage, damage / 20 * maxDamage,
            //   damage / 40 * maxDamage, damage / 50 * maxDamage,
            //   damage / 0.5f * maxDamage, damage / 0.6f * maxDamage,
            //   color
            //);
            int numMin = 0, numMax = 0, speedMin = 0, speedMax = 0;
            float sizeMin = 0f, sizeMax = 0f;

            #region Vodka
            if (damage < 50)
            {
                numMin = 1;
                numMax = 1;
                speedMin = 5;
                speedMax = 10;
                sizeMin = 0.2f;
                sizeMax = 0.3f;
            }
            else if (damage < 100)
            {
                numMin = 2;
                numMax = 3;
                speedMin = 7;
                speedMax = 15;
                sizeMin = 0.2f;
                sizeMax = 0.3f;
            }
            else if (damage < 200)
            {
                numMin = 4;
                numMax = 5;
                speedMin = 10;
                speedMax = 20;
                sizeMin = 0.2f;
                sizeMax = 0.3f;
            }
            else
            {
                numMin = 6;
                numMax = 10;
                speedMin = 15;
                speedMax = 35;
                sizeMin = 0.3f;
                sizeMax = 0.4f;
            }
            #endregion

            insta.GetComponent<SplashManagerInside>().SetParticleSystem(
               numMin, numMax,
               speedMin, speedMax,
               sizeMin, sizeMax,
               color
            );

            insta.SetActive(true);
        }
    }

    public void SpawnDamageParticles(int damage, Colors color, Vector3 position, Unit.Side side)
    {
        SpawnDamageParticles(damage, color, position, (side == Unit.Side.ALLY ? new Vector3(-45, -90, 90) : new Vector3(-135, -90, 90)));
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
                        if (hitted.maxHealth > 3000)
                            needed = castle;
                        else
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

    private IEnumerator Gain()
    {
        Debug.Log("ouiiii");
        while(true)
        {
            yield return new WaitForSeconds(pumpTime);
            ShopManager.instance.Gain(passivePump);
        }
    }

    public void Defeat()
    {
        defeatScreen.GetComponent<PauseMenu>().TimeScaleToZero();
        defeatScreen.SetActive(true);
    }

    public void Victory()
    {
        victoryScreen.GetComponent<PauseMenu>().TimeScaleToZero();
        victoryScreen.SetActive(true);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ToggleGO(GameObject taureau)
    {
        taureau.SetActive(!taureau.activeSelf);
    }

    public void ToggleParticleEnabled()
    {
        particleEnabled = !particleEnabled;
    }
}
