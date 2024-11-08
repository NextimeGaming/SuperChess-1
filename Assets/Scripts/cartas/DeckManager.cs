using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Carta> cartasDisponiveis; // Lista de todas as cartas possíveis
    public List<Carta> cartasNoDeck; // Cartas atuais no deck

    private void Start()
    {
        // Preenche o deck com as cartas disponíveis
        CriarDeck();
    }

    // Cria um deck de cartas 
    private void CriarDeck()
    {
        cartasNoDeck = new List<Carta>(cartasDisponiveis);
        EmbaralharDeck();
    }

    // Função para embaralhar o deck
    public void EmbaralharDeck()
    {
        for (int i = 0; i < cartasNoDeck.Count; i++)
        {
            Carta temp = cartasNoDeck[i];
            int randomIndex = Random.Range(i, cartasNoDeck.Count);
            cartasNoDeck[i] = cartasNoDeck[randomIndex];
            cartasNoDeck[randomIndex] = temp;
        }
    }

    // Comprar uma carta do deck
    public Carta ComprarCarta()
    {
        if (cartasNoDeck.Count > 0)
        {
            Carta cartaComprada = cartasNoDeck[0];
            cartasNoDeck.RemoveAt(0);
            return cartaComprada;
        }
        return null;
    }

    // Jogar uma carta
    public void JogarCarta(Carta carta, TabuleiroDamas tabuleiro, PieceManager pieceManager)
    {
        carta.UsarCarta(tabuleiro, pieceManager);
    }
}
