using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Turn { player1, player2 }
    public Turn currentTurn;

    public WhitePawnManager whitePawnManager;
    public whiteArcherManager whiteArcherManager;

    void Start()
    {
        currentTurn = Turn.player1; // Inicia com o jogador 1
        StartTurn();
    }

    public void StartTurn()
    {
        // Ativa as acoes do jogador atual
        if (currentTurn == Turn.player1)
        {
            whitePawnManager.EnableActions();
        }
        else
        {
            whiteArcherManager.EnableActions();
        }
    }

    public void EndTurn()
    {
        // Desativa as acoes do jogador atual
        if (currentTurn == Turn.player1)
        {
            whitePawnManager.DisableActions();
        }
        else
        {
            whiteArcherManager.DisableActions();
        }

        // Troca de turno
        currentTurn = (currentTurn == Turn.player1) ? Turn.player2 : Turn.player1;
        StartTurn(); // Inicie o turno do próximo jogador
    }
}
