using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public TurnManager turnManager;
    public AttackCircleManager attackCircleManager;
    public WhitePawnManager whitePawnManager;
    public whiteArcherManager whiteArcherManager;

    void Start()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        attackCircleManager = FindAnyObjectByType<AttackCircleManager>();
        whitePawnManager = FindAnyObjectByType<WhitePawnManager>();
        whiteArcherManager = FindAnyObjectByType<whiteArcherManager>();
    }

    public void Attack(Vector3 targetPosition)
    {
        if (turnManager.currentTurn == TurnManager.Turn.player1)
        {
            if (whiteArcherManager.IsInAttackRange(targetPosition))
            {
                // Implementa a lógica de ataque
                whiteArcherManager.IsInAttackRange(targetPosition);
                turnManager.EndTurn(); // Finaliza o turno após atacar
            }
        }
    }

    void Update()
    {
        // Exemplo de como o ataque pode ser chamado
        if (Input.GetMouseButtonDown(0) && turnManager.currentTurn == TurnManager.Turn.player1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Casa"))
                {
                    Vector3 targetPosition = hit.point;
                    Attack(targetPosition);
                }
            }
        }
    }
}
