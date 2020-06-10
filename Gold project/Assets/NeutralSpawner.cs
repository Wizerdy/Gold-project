using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralSpawner : MonoBehaviour
{
    [HideInInspector] public bool towerIsTaken = false;
    [SerializeField]private NeutralSpawner isLinkedTo;

    [Space]
    public int nbOfSlime = 3;
    public float cdBetweenSlime = .5f;
    public float cdBetweenWave = 5;
    private bool waitingBetweenWave = false;
    [SerializeField] private float waitForIt;

    [Space]
    public GameObject target;
    private Tower towerInstance;

    [Space]
    public NeutralSpawner[] lookOut;
    private List<GameObject> isTaken = new List<GameObject>();

    private bool wait;

    void Start()
    {
        wait = false;
        towerInstance = GetComponent<Tower>();

        if(lookOut.Length > 0)
        {
            foreach(NeutralSpawner lookFor in lookOut)
            {
                lookFor.isLinkedTo = GetComponent<NeutralSpawner>();
            }
        }

        StartCoroutine("Wait", waitForIt);
    }

    void Update()
    {
        if(lookOut.Length > 0 && isTaken.Count > 0 && towerInstance.side == Unit.Side.NEUTRAL)
        {
            foreach (GameObject tower in isTaken)
            {
                StartCoroutine(SpawnNeutralForce(tower.GetComponent<NeutralSpawner>().target));
            }
        }

        if(lookOut.Length == 0 && towerInstance.side == Unit.Side.NEUTRAL && wait)
        {
            StartCoroutine(SpawnNeutralForce(target));
        }

        //Je veux pas rajouter deux ligne dans le script de Flo, j'ai peur de me faire tapper :'(
        if(towerInstance.side != Unit.Side.NEUTRAL && towerIsTaken == false)
        {
            if(lookOut.Length == 0)
                isLinkedTo.isTaken.Add(gameObject);
            towerIsTaken = true;
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnNeutralForce(GameObject target)
    {       
        if(waitingBetweenWave == false)
        {
            waitingBetweenWave = true;
            yield return new WaitForSeconds(cdBetweenWave);

            for(int i = 0; i < nbOfSlime; i++)
            {
                GameObject obj = GameManager.instance.InstantiateUnit(GameManager.instance.units["WHITE"], Unit.Side.NEUTRAL, towerInstance.spawnPoint.position);
                obj.GetComponent<Knight>().target = target.transform;
                    
                switch(target.name)
                {
                    case "Ally":
                        obj.transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case "Enemies":
                        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                }


                yield return new WaitForSeconds(cdBetweenSlime);
            }

            yield return new WaitForSeconds(1f);
            waitingBetweenWave = false;
        }
    }


    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        wait = true;
    }
}
