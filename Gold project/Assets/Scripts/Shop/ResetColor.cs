using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetColor : MonoBehaviour
{
    public GameObject white;

    public void WhiteColor(GameObject slot)
    {
            Destroy(slot.transform.GetChild(0).gameObject);
            Instantiate(white, slot.transform).GetComponent<DragHandeler>().barren = true;
    }
}
