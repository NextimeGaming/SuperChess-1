using System;
using UnityEngine;

public class CardBack : MonoBehaviour
{
    public GameObject cardBack;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if(DisplayCard.staticCardBack == true)
        {
            cardBack.SetActive(true);
        }
        else
        {
            cardBack.SetActive(false);
        }
    }

    private static void SetActive(bool v)
    {
        
    }
}
