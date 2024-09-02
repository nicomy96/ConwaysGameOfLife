using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class PatternUI : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] GameObject nextPatterns;
    [SerializeField] GameObject prevPatterns;
    bool areNextPatterns = true;
    // Start is called before the first frame update

    private void Start()
    {
        areNextPatterns = true;
        scrollbar.value = 0;
    }

    private void Update()
    {
        SetVisibilityOfNavigationButtons();
    }

    private void SetVisibilityOfNavigationButtons()
    {
        if(scrollbar.value < 0.25)
        {
            prevPatterns.SetActive(false);
        }
        else
        {
            prevPatterns.SetActive(true);
        }
        if(scrollbar.value > 0.75)
        {
            nextPatterns.SetActive(false);
        }
        else
        {
            nextPatterns.SetActive(true);
        }
    }

    public void ScrollerNextState(int value)
    {
        scrollbar.value = value;
        areNextPatterns = !areNextPatterns;
        nextPatterns.SetActive(areNextPatterns);
        prevPatterns.SetActive(!areNextPatterns);
    }
}
