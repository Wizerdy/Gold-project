using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static float Map(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }

    public static Vector2 Map(float value, float min1, float max1, Vector2 min2, Vector2 max2)
    {
        return new Vector2(min2.x + (value - min1) * (max2.x - min2.x) / (max1 - min1), min2.y + (value - min1) * (max2.y - min2.y) / (max1 - min1));
    }

    public static void AddOiL(GameObject obj, int add)
    {
        SpriteRenderer[] sprRenders = obj.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprRenders.Length; i++)
            sprRenders[i].sortingOrder += add;
    }
}
