

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public List<Card> displayCard = new List<Card>();
    public int displayId;

    public int id;
    public string cardName;
    public int cost;
    public int power;
    public int health;
    public int move;
    public string cardDescription;
    public Sprite spriteImage;

   
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text powerText;
    public TMP_Text healthText;
    public TMP_Text moveText;
    public TMP_Text descriptionText;
    public UnityEngine.UI.Image artImage;

    public bool CardBack;
    public static bool staticCardBack;

    void Start()
    {
        if (CardDataBase.cardList.Count > displayId)
        {
            displayCard.Add(CardDataBase.cardList[displayId]);
        }
    }

    void Update()
    {
        if (displayCard.Count > 0)
        {
            id = displayCard[0].id;
            cardName = displayCard[0].cardName;
            cost = displayCard[0].cost;
            power = displayCard[0].power;
            health = displayCard[0].health;
            move = displayCard[0].move;
            cardDescription = displayCard[0].cardDescription;
            spriteImage = displayCard[0].spriteImage;

            nameText.text = cardName;
            costText.text = cost.ToString();
            powerText.text = power.ToString();
            healthText.text = health.ToString();
            moveText.text = move.ToString();
            descriptionText.text = cardDescription;
            artImage.sprite = spriteImage; 
        }

        staticCardBack = CardBack;
    }

}