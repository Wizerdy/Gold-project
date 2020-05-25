using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleSilme : Knight
{
    public GameObject SlimeToSpawn;
    public int numberToSpawn;
    [Range(0, 5f)] public float spawnOffset;

    protected override void Die()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            //GameObject insta = Instantiate(SlimeToSpawn, transform.position + new Vector3(spawnOffset * i, 0), Quaternion.identity);
            GameManager.instance.InstantiateUnit(SlimeToSpawn, side, transform.position + new Vector3(spawnOffset * i, 0));
        }

        base.Die();
    }
}
