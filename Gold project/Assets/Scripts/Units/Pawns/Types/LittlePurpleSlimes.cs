using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlePurpleSlimes : Pawn
{
    public float lifeTime;

    protected override void Start()
    {
        base.Start();
        StartCoroutine("Live", lifeTime);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.InsideLayer(collision.gameObject.layer, GameManager.instance.floorLayer) && transform.position.y != GameManager.instance.allyParent.position.y)
        {
            Debug.LogError("Touchdooooown");
            transform.position = new Vector3(transform.position.x, GameManager.instance.allyParent.position.y, transform.position.z);
            transform.eulerAngles = (side == Side.ALLY ? Vector3.zero : new Vector3(180, 0, 0));
            GetComponent<Pawn>().enabled = true;
            Destroy(GetComponent<Rigidbody2D>());
            gameObject.layer = GameManager.instance.allyParent.gameObject.layer;
        }
    }

    IEnumerator Live(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
