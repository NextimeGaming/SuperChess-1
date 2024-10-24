using UnityEngine;


public class TurnManager : MonoBehaviour
{
    //public enum turn { geratabuleiro = 0,turnplayer1 = 1,turnplayer2 = 2};
    public enum turn { player1, player2 };
    public turn currentTurn =  turn.player1;

    public WhitePawnManager whitePawnManager;
    public whiteArcherManager   whiteArcherManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        whitePawnManager = FindAnyObjectByType<WhitePawnManager>();
        whiteArcherManager = FindAnyObjectByType<whiteArcherManager>();
        
    }

    // Update is called once per frame
    public void Update()
    {
        //EndTurn();
    }
    public void EndTurn()
    {
        if (currentTurn == turn.player1)
            currentTurn = turn.player2;

        else  
        { currentTurn = turn.player1; 
            Debug.Log(turn.player1);
        
        }
        

        
    }
}
