using UnityEngine;

public class BatlleManager : MonoBehaviour
{
    public TurnManager TurnManager;
    public AttackCircleManager attackCircleManager;
    public WhitePawnManager whitePawnManager;
    public whiteArcherManager whiteArcherManager;   
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnManager = FindAnyObjectByType<TurnManager>();
        attackCircleManager = FindAnyObjectByType<AttackCircleManager>();
        whitePawnManager = FindAnyObjectByType<WhitePawnManager>();
        whiteArcherManager = FindAnyObjectByType< whiteArcherManager>();
    }
    public void Attack (Vector3 targetPosition)
    {
        if (TurnManager.currentTurn == TurnManager.turn.player1)
        {
            Vector2Int[] dsa = whiteArcherManager.asd;
            foreach (var dsa2 in dsa)
            {
                
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
