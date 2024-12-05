

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public int id;
    public string cardName;
    public int cost; // custo de energia
    public int power;
    public int health;
    public int move;
    public string cardDescription; // descrição da carta
    public Sprite spriteImage; // Imagem da carta

    public Card()
    {
        
    }

    public Card(int Id, string CardName, int Cost, int Power,int Health, int Move, string CardDescription, Sprite SpriteImage)
    {
        id = Id;
        cardName = CardName;
        cost = Cost;
        power = Power;
        health = Health;
        move = Move;
        cardDescription = CardDescription;
        spriteImage = SpriteImage; 
    }
}