using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralSpawner : MonoBehaviour
{
    public bool towerIsTaken = false;
    private NeutralSpawner isLinkedTo;

    [Space]
    public int nbOfSlime = 3;
    public float cdBetweenSlime = .5f;
    public float cdBetweenWave = 5;
    private bool waitingBetweenWave = false;

    [Space]
    public GameObject target;
    private Tower towerInstance;

    [Space]
    public NeutralSpawner[] lookOut;
    private List<GameObject> isTaken;

    void Start()
    {
        towerInstance = GetComponent<Tower>();

        if(lookOut.Length > 0)
        {
            foreach(NeutralSpawner lookFor in lookOut)
            {
                lookFor.isLinkedTo = GetComponent<NeutralSpawner>();
            }
        }
    }

    void Update()
    {
        if(lookOut.Length > 0 && isTaken.Count > 0)
        {
            foreach (GameObject tower in isTaken)
            {
                StartCoroutine(SpawnNeutralForce(tower.GetComponent<NeutralSpawner>().target));
            }
        }

        if(lookOut.Length == 0 && towerInstance.side != Unit.Side.NEUTRAL)
        {

            StartCoroutine(SpawnNeutralForce(target));
        }

        //Je veux pas rajouter deux ligne dans le script de Flo, j'ai peur de me faire tapper :'(
        if(towerInstance.side != Unit.Side.NEUTRAL && towerIsTaken == false)
        {
            isLinkedTo.isTaken.Add(gameObject);
            towerIsTaken = true;
        }
    }

    IEnumerator SpawnNeutralForce(GameObject target)
    {
        if(waitingBetweenWave == false)
        {
            waitingBetweenWave = true;


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

            yield return new WaitForSeconds(cdBetweenWave);
            waitingBetweenWave = false;
        }
    }

}
