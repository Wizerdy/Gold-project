using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCoroutine : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private bool fade = true;
    [SerializeField] private SpriteRenderer sprRend;

    void Start()
    {
        StartCoroutine("Life", lifeTime);
    }

    private IEnumerator Life(float time)
    {
        yield return new WaitForSeconds(time);
        if (fade)
            StartCoroutine("fade");
        else
            Destroy(gameObject);
    }

    private IEnumerator Fade()
    {
        float timePassed = 0f;

        while(timePassed < 1f)
        {
            yield return new WaitForSeconds(0.05f);
            timePassed += 0.05f;
            sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, Tools.Map(timePassed, 0f, 1f, 0, sprRend.color.a));
        }
        Destroy(gameObject);
    }
}
