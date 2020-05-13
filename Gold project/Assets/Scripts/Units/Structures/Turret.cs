using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Structure
{
    public GameObject ammo;

    public Transform sprite;

    protected override void Attack(GameObject target)
    {
        base.Attack(target);

        Vector2 dir = target.transform.position - sprite.transform.position;

        Quaternion rotation = Quaternion.LookRotation(dir, Vector2.right);
        sprite.eulerAngles = new Vector3(0, 0, -rotation.eulerAngles.x);
        //Debug.Log(sprite.name + " .. " + rotation + " .. " + rotation.eulerAngles);
        Instantiate(ammo, sprite.position, rotation);
    }

    private void Update()
    {
        if (CheckAttackRange() && canAttack)
        {
            Attack(hit[hitIndex].gameObject);
        }
    }
}
