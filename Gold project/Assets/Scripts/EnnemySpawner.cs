using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;

    private float deltaTime;
    public float intervalle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        if(deltaTime >= intervalle)
        {
            SpawnEnnemy();
        }

    }

    void SpawnEnnemy()
    {
        //Instantiate enemy prefab at spawnPoint
        //Debug.Log("Spawning ennemies");
        if(GameObject.FindGameObjectsWithTag("Enemy").Length < 10)
        {
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }

        deltaTime = 0;
    }
}
