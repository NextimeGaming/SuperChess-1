using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        cardList.Add(new Card(0, "None", 0, 0, 0, 0, "None", Resources.Load<Sprite>("Personagem")));
        cardList.Add(new Card(1, "Arqueiro", 1, 4, 5, 2, "Transforma um peão em Arqueiro", Resources.Load<Sprite>("Arqueiro")));
        cardList.Add(new Card(2, "Guerreiro", 6, 4, 8, 4, "Transforma um peão em Guerreiro", Resources.Load<Sprite>("Guerreiro")));
        cardList.Add(new Card(3, "Ladino", 1, 4, 5, 4, "Transforma um peão em Ladino", Resources.Load<Sprite>("Ladino")));
        cardList.Add(new Card(4, "Mago", 1, 4, 5, 1, "Transforma um peão em Mago", Resources.Load<Sprite>("Mago")));
        cardList.Add(new Card(5, "Druida", 1, 2, 6 ,2, "Transforma um peão em Druida", Resources.Load<Sprite>("Druida")));
        cardList.Add(new Card(6, "Caçador", 7, 3, 7, 3, "Transforma um peão em Caçador", Resources.Load<Sprite>("Caçador")));
       
    }






}
