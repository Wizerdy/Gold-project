using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    [Header("Stats")]
    public int health;

    [Space]

    [Header("Slot management")]
    public Structure[] slots;
    public int slotPrice; 
}
