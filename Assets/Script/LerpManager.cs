using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class LerpManager : MonoBehaviour
{

    [SerializeField] private GameObject reference;
    [SerializeField] private List<LerpFunction> lerpList;
    private bool isActive;
    private bool areCoroutinesActived = false;

    private void Awake()
    {
        isActive = true;
        areCoroutinesActived = false;
        //disable all at first 
        foreach (LerpFunction lerp in lerpList)
        {
            lerp.isActive = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (!areCoroutinesActived)
            {
                foreach (LerpFunction lerp in lerpList)
                {
                    lerp.Init();
                    StartCoroutine(lerp.ExcuteLerp(reference));
                }
                areCoroutinesActived = true; 
            }
        }
        else
        {
            foreach (LerpFunction lerp in lerpList)
            {
                lerp.Reset();
            }
            StopAllCoroutines();
        }
    }

    private void OnDisable()
    {
        if (isActive)
        {
            isActive = false;
            foreach (LerpFunction lerp in lerpList)
            {
                lerp.Reset();
            }
            StopAllCoroutines();
        }
    }

    public void SetActiveStatus(bool condition)
    {
        isActive = condition;
    }
}
