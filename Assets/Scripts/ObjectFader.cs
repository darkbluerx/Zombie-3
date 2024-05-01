using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    [Header("Fade settings")]

    [Tooltip("Speed at which object fades")] public float fadeSpeed = 10f;
    public float fadeAmount = 0.5f;

    float orginalOpacity;

    Material material;

    public bool DoFade = false;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Start()
    {    
        orginalOpacity = material.color.a;  
    }

    private void Update()
    {
        if (DoFade)
        {
            Fade();
        }
        else
        {
            ResetFade();
        }
    }

    private void Fade()
    {
        Color curreColor = material.color;
        Color smootColor = new Color(curreColor.r, curreColor.g, curreColor.b, Mathf.Lerp(curreColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
        material.color = smootColor;
    }

    public void ResetFade()
    {
        Color curreColor = material.color;
        Color smootColor = new Color(curreColor.r, curreColor.g, curreColor.b, Mathf.Lerp(curreColor.a, orginalOpacity, fadeSpeed * Time.deltaTime));
        material.color = smootColor;
    }
}
