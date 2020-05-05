using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public enum _enemyColor
    {
        GREEN,
        BLUE,
        YELLOW,
        RED
    };

    public _enemyColor enemyColor;

    //When it spawns
    private void Awake()
    {
        SetAndApplyColor();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToCastle();
    }

    void SetAndApplyColor()
    {
        //Find the way to put it into EnnemySpawner
        enemyColor = (_enemyColor)Random.Range(0, /*Trouver un moyen d'obtenir la taille de l'enum*/ 4);

        switch (enemyColor)
        {
            case _enemyColor.GREEN:
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                }

            case _enemyColor.BLUE:
                {
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                }

            case _enemyColor.YELLOW:
                {
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                }

            case _enemyColor.RED:
                {
                    GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }

            default:
                {
                    enemyColor = _enemyColor.BLUE;
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                }
        }
    }

    void MoveToCastle()
    {
        Vector3 direction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCastle>().castleDoors.transform.position - transform.position;
        //Temporary solution, get the furthest avant-poste du joueur in the future
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy touche le player");
        }
    }
}
