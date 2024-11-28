

using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> container = new List<Card>();
    public int deckSize;
    public List<Card> deck = new List<Card>();
    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    void Start()
    {
        
        for (int i = 0; i < 40; i++)
        {
            int x = Random.Range(1, 7);
            deck.Add(CardDataBase.cardList[x]);
        }

        
        ShuffleDeck();
    }

    void Update()
    {
        if (deckSize < 40)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 20)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 10)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 2)
        {
            cardInDeck4.SetActive(false);
        }
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }
    }
}