﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public float time = 1f;
    [HideInInspector]public Material mat;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        mat = img.material;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mat.SetVector("_Wavy", new Vector4(.01f, .003f));
            mat.SetFloat("_Border", 10f);
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            mat.SetVector("_Wavy", new Vector4(-.01f, .003f));
            mat.SetFloat("_Border", -15f);
            StartCoroutine(TransitionEnd());
        }
    }

    public void CallTransition(string transition)
    {
        StartCoroutine("Transition"+transition);
    }

    public IEnumerator TransitionStart()
    {
        float elapsed = 0f;
        img.raycastTarget = true;
        

        while (elapsed < time)
        {
            mat.SetFloat("_Border", Mathf.Lerp(10f, -10f, (elapsed/time)));
            elapsed += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(time);
        Debug.Log("Switch scene");
        SceneManager.LoadScene("Game");
    }

    public IEnumerator TransitionEnd()
    {
        float elapsed = 0f;
        img.raycastTarget = true;        

        while (elapsed < time)
        {
            mat.SetFloat("_Border", Mathf.Lerp(-15f, 15f, (elapsed / time)));
            elapsed += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(time);
        img.raycastTarget = false;

    }

}
