using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Structure
{
    public GameObject ammo;
    public float ammoSpeed;

    public Transform sprite;

    protected override void Attack(GameObject target)
    {
        base.Attack(target);

        Vector2 dir = (target.transform.position - 
            (sprite.transform.position + new Vector3(1, 0, 0) * (target.GetComponent<Pawn>() != null ? (target.GetComponent<Pawn>().speed / 4) : 1))
        );

        Quaternion rotation = Quaternion.LookRotation(dir, Vector2.right);
        sprite.eulerAngles = new Vector3(0, 0, -rotation.eulerAngles.x);
        //Debug.Log(sprite.name + " .. " + rotation + " .. " + rotation.eulerAngles);
        GameObject insta = Instantiate(ammo, sprite.position, rotation);
        insta.transform.eulerAngles = new Vector3(0, 0, -rotation.eulerAngles.x);
        insta.GetComponent<Rigidbody2D>().AddForce(dir * ammoSpeed);
        insta.GetComponent<ArrowController>().damage = DealDamage();
        insta.GetComponent<ArrowController>().side = side;
    }

    private void Update()
    {
        if (CheckAttackRange() && canAttack)
        {
            Attack(hit[hitIndex].gameObject);
        }
    }
}
