using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    public Vector2 timeToSpawn;

    private IEnumerator SpawnUnit()
    {
        while (true)
        {
            string name = ((Colors)Random.Range(0, 7)).ToString();
            GameManager.instance.InstantiateUnit(GameManager.instance.units[name], Unit.Side.ENEMY, GameManager.instance.enemyParent);
            yield return new WaitForSeconds(Random.Range((float)timeToSpawn.x, (float)timeToSpawn.y));
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnUnit());
    }
}
