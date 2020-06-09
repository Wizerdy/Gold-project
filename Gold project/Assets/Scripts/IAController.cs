using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    private enum Behaviour { AGRESSIVE, DEFENSIVE }


    public int money;

    private Behaviour behaviour = Behaviour.DEFENSIVE;

    public List<Tower> structures;

    [Header("Options")]
    public float actionTime;
    public float spawnOffset;
    public float defActionTime;

    [Range(0, 100)] public int defChance = 100;
    [Range(0f, 1f)] private float defReduction = 0.94f;
    private int atkReduction = 0;

    [HideInInspector] public int pumpCount = 0;
    [HideInInspector] public int turretCount = 0;

    private int moneyToUse;
    private int unitToSpawn;

    private Coroutine action;

    private void Start()
    {
        StartCoroutine(Pump());
        StartCoroutine(Act());
        ChooseBehaviour();
    }

    private IEnumerator Act()
    {
        while(true)
        {
            yield return new WaitForSeconds(actionTime);
            ChooseBehaviour();

            if(defChance > 10)
                defChance = Mathf.FloorToInt(defChance * defReduction);
        }
    }

    private void ChooseBehaviour()
    {
        int rand = Random.Range(0, 100);

        if(rand < defChance + ((100 - defChance) * (atkReduction / 4)))
            ChangeBehaviour(Behaviour.DEFENSIVE);
        else
            ChangeBehaviour(Behaviour.AGRESSIVE);
    }

    private void ChangeBehaviour(Behaviour behaviour)
    {
        this.behaviour = behaviour;

        if(action != null)
            StopCoroutine(action);

        switch (behaviour)
        {
            case Behaviour.DEFENSIVE:
                atkReduction = 0;
                action = StartCoroutine(Defensive());
                Debug.Log("-- Def Mode !");
                break;
            case Behaviour.AGRESSIVE:
                if (atkReduction < 4)
                    atkReduction++;

                moneyToUse = Mathf.FloorToInt(money * Random.Range(0f, 1f));
                unitToSpawn = Random.Range(0, 8);

                while (GetUnit((Colors)unitToSpawn).GetComponent<Unit>().cost > moneyToUse)
                {
                    if (unitToSpawn > 1)
                        unitToSpawn--;
                    else
                    {
                        ChangeBehaviour(Behaviour.DEFENSIVE);
                        return;
                    }
                }
                action = StartCoroutine(Agressive());
                Debug.Log("-- Atk Mode !");
                break;
        }
    }

    private IEnumerator Agressive()
    {
        while (moneyToUse > 0)
        {
            yield return new WaitForSeconds(spawnOffset);

            int rand = Random.Range(0, unitToSpawn + 1);
            if (Pay(GetUnit((Colors)rand).GetComponent<Unit>().cost))
            {
                SpawnUnit((Colors)rand);
                moneyToUse -= GetUnit((Colors)rand).GetComponent<Unit>().cost;
                Debug.Log("+ Spawned : " + ((Colors)rand).ToString());
            }
        }
    }

    private IEnumerator Defensive()
    {
        while (true)
        {
            yield return new WaitForSeconds(defActionTime);

            int rand = Random.Range(0, 100);
            if (rand < 20 + ((100 - 20) * (turretCount / 7)))
            {
                ActivateAPump();
            }
            else
            {

                int turretIndex = 0;

                for (int i = 0; i < 8; i++)
                    if (GetTurret((Colors)i).GetComponent<Unit>().cost <= money)
                        turretIndex = i;
                    else
                        break;

                Transform parent = SearchTurretSlot(turretIndex);
                if (parent != null && Pay(GetTurret((Colors)turretIndex).GetComponent<Unit>().cost))
                {
                    if (parent.childCount > 0)
                    {
                        for (int i = 0; i < parent.childCount; i++)
                            Destroy(parent.GetChild(i).gameObject);
                    }

                    GameObject insta = GameManager.instance.InstantiateUnit(GetTurret((Colors)turretIndex), Unit.Side.ENEMY, parent);
                    if (parent.GetComponent<OrderInLayer>() != null)
                    {
                        int add = parent.GetComponent<OrderInLayer>().orderInLayer;

                        Tools.AddOiL(insta, add);
                    }

                    turretCount++;
                    Debug.Log("+ Turret");
                }
            }
        }
    }

    private IEnumerator Pump()
    {
        while(true)
        {
            yield return new WaitForSeconds(GameManager.instance.pumpTime);
            Gain(GameManager.instance.passivePump + GameManager.instance.pumpAmount * pumpCount);
        }
    }

    private GameObject GetUnit(Colors color)
    {
        return GameManager.instance.units[color.ToString()];
    }

    private GameObject GetTurret(Colors color)
    {
        return GameManager.instance.units["T_" + color.ToString()];
    }

    private void SpawnUnit(Colors color)
    {
        for (int i = structures.Count - 1; i > -1; i--)
        {
            if (structures[i].side == Unit.Side.ENEMY)
            {
                GameManager.instance.InstantiateUnit(GetUnit(color), Unit.Side.ENEMY, structures[i].spawnPoint.position);
                return;
            }
        }
    }

    private void ActivateAPump()
    {
        if (money >= GameManager.instance.pumpCost)
            for (int i = 0; i < structures.Count; i++)
                if(structures[i].side == Unit.Side.ENEMY)
                    for (int j = 0; j < structures[i].pumps.Count; j++)
                        if (!structures[i].pumps[j].gameObject.activeSelf)
                        {
                            Pay(GameManager.instance.pumpCost);
                            structures[i].pumps[j].GetComponent<Pump>().enabled = false;
                            structures[i].pumps[j].gameObject.SetActive(true);
                            Debug.Log("+ Pump");
                            pumpCount++;
                            return;
                        }
    }
    
    private Transform SearchTurretSlot(int upgrade)
    {
        for (int i = 0; i < structures.Count; i++)
            if (structures[i].GetComponent<Tower>().side == Unit.Side.ENEMY)
                for (int j = 0; j < structures[i].turrets.Count; j++)
                    if (structures[i].turrets[j].childCount == 0 || (int)structures[i].turrets[j].GetChild(0).GetComponent<Turret>().color < upgrade)
                        return structures[i].turrets[j];

        return null;
    }

    public void Gain(int amount)
    {
        money += amount;
    }

    public bool Pay(int amount)
    {
        if (money >= amount)
        {
            Debug.LogWarning("Pay : " + amount);
            money -= amount;
            return true;
        }
        return false;
    }
}
