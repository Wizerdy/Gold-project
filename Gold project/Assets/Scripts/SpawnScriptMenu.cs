using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScriptMenu : MonoBehaviour
{
    public float minimum = 1;
    public float maximum = 3;
    private bool lockslime = true;
    private bool chacunsontour = true;




    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (lockslime == true)
        {
            if (chacunsontour == true)
            {
                StartCoroutine(GenererSlime());
                    chacunsontour = false;
            }    
            else
            {
                StartCoroutine(GenererSlimp());
                chacunsontour = true;
            }
        }

    }

    private IEnumerator GenererSlime()
    {
        lockslime = false;
        GameManager.instance.InstantiateUnit(GameManager.instance.units["WHITE"], Unit.Side.ALLY, GameManager.instance.allyParent.position);
        float timer = Random.Range(minimum, maximum);
        yield return new WaitForSeconds(timer);
        lockslime = true;

    }
    private IEnumerator GenererSlimp()
    {
        lockslime = false;
        GameManager.instance.InstantiateUnit(GameManager.instance.units["WHITE Slimp"], Unit.Side.ENEMY, GameManager.instance.enemyParent.position);
        float timer = Random.Range(minimum, maximum);
        yield return new WaitForSeconds(timer);
        lockslime = true;

    }
}
