using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Structure
{
    public GameObject ammo;
    public float ammoSpeed;
    public float offSet = 1f;

    public Transform sprite;

    protected override void Attack(GameObject target)
    {
        base.Attack(target);

        Vector2 dir = (target.transform.position -
            (sprite.transform.position + new Vector3(1 * offSet, 0, 0) * 
            (target.GetComponent<Pawn>() != null && !target.GetComponent<Pawn>().immobilize ? (target.GetComponent<Pawn>().speed / 4) : 1))
        );

        Quaternion rotation = Quaternion.LookRotation(dir, Vector2.right);
        sprite.eulerAngles = new Vector3(sprite.eulerAngles.x, sprite.eulerAngles.y, -rotation.eulerAngles.x);
        //Debug.Log(sprite.name + " .. " + rotation + " .. " + rotation.eulerAngles);
        GameObject insta = null;
        if (ammo.GetComponent<Pawn>() == null)
        {
            insta = Instantiate(ammo, sprite.position, rotation);

            if (insta.GetComponent<ArrowController>() != null)
            {
                insta.GetComponent<ArrowController>().damage = DealDamage();
                insta.GetComponent<ArrowController>().side = side;
            }
        } else
        {
            insta = GameManager.instance.InstantiateUnit(ammo, side, transform.position);
            insta.GetComponent<Pawn>().enabled = false;
        }

        insta.transform.rotation = rotation;
        insta.transform.eulerAngles = new Vector3(0, 0, -rotation.eulerAngles.x);
        insta.GetComponent<Rigidbody2D>().AddForce(dir * ammoSpeed);

        Debug.DrawLine(transform.position, (Vector2)transform.position + dir, Color.red, attackSpeed);
    }

    private void Update()
    {
        if (CheckAttackRange() && canAttack)
        {
            Attack(hit[hitIndex].gameObject);
        }
    }
}
