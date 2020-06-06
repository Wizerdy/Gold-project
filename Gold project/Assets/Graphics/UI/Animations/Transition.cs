using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Transition : MonoBehaviour
{
    public float time = 1f;
    [HideInInspector]public Material mat;
    private Image img;
    public string SceneName = "Game";

    private void Start()
    {
        img = GetComponent<Image>();
        mat = img.material;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mat.SetVector("_Wavy", new Vector4(.01f, .003f));
            mat.SetFloat("_Border", 10f);
        }

        if (SceneManager.GetActiveScene().name == SceneName)
        {
            mat.SetVector("_Wavy", new Vector4(-.01f, .003f));
            mat.SetFloat("_Border", -15f);
            StartCoroutine(TransitionEnd());
        }
    }

    public void CallTransition()
    {
        StartCoroutine(TransitionStart());
    }

    public IEnumerator TransitionStart()
    {
        img.raycastTarget = true;
        mat.SetVector("_Wavy", new Vector4(.01f, .003f));
        mat.SetFloat("_Border", 15f);

        mat.DOFloat(-15f, "_Border", 1f);     

        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneName);
    }

    public IEnumerator TransitionEnd()
    {
        img.raycastTarget = true;

        mat.DOFloat(15f, "_Border", 1f);


        yield return new WaitForSeconds(time);
        img.raycastTarget = false;

    }

}
