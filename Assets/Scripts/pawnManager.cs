using UnityEngine;
using System.Collections;

public class PawnManager : MonoBehaviour
{
    private static PawnManager selectedPawn = null; // Pe�a atualmente selecionada
    private static PawnManager pecadiminuindo = null; // pe�a vai diminuir de tamanho

    private bool mouseOver = false;
    public Color hoverColor;
    private Renderer rend;
    private Color startColor;

    public float moveSpeed = 2f; // Velocidade de movimenta��o
    private Vector3 targetPosition; // Posi��o  da pe�a para movimenta��o
    private bool isMoving = false, isShrinking = false; // Flag para verificar se a pe�a est� se movendo
    public TabuleiroDamas tabuleiro;

    private Vector3 scaleChange;

    private void Start()
    {
        scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
        tabuleiro = FindObjectOfType(typeof(TabuleiroDamas)) as TabuleiroDamas;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (selectedPawn != this) // N�o altera a cor se a pe�a estiver selecionada
        {
            rend.material.color = startColor;
        }
        mouseOver = false;
    }

    private void OnMouseDown()
    {
        // Se a pe�a est� sob o cursor e � clicada, verifica o status de sele��o
        if (mouseOver)
        {
            if (selectedPawn == this)
            {
                DeselectPawn();
            }
            else
            {
                SelectPawn();
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            isShrinking = true;
        }

        if (isShrinking)
        {
            while(transform.localScale.x >= 0.2f)
            {
                transform.localScale = -scaleChange;
            }
        }

        // Mova a pe�a em dire��o a casa
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                tabuleiro.desocupaCasa((int)transform.position.x, (int)transform.position.y);
                tabuleiro.ocupaCasa((int)targetPosition.x, (int)targetPosition.y);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            }
            else
            {
                transform.position = targetPosition;
                isMoving = false; // Interrompe o movimento quando chega a casa
            }
        }
    }

    // Seleciona a pe�a e define a posi��o da casa
    private void SelectPawn()
    {
        // Se j� houver outra pe�a se movendo, n�o faz nada
        if (isMoving || selectedPawn != null)
            return;

        // Marca a pe�a como selecionada
        selectedPawn = this;
        rend.material.color = hoverColor; // Mant�m a cor de sele��o

        //pecadiminuindo = this;

        // Inicia a movimenta��o
        StartCoroutine(WaitForClick());
    }

    private void DeselectPawn()
    {
        selectedPawn = null;
        rend.material.color = startColor; // Restaura a cor original
    }

    // Aguarda um clique em uma posi��o do tabuleiro
    private IEnumerator WaitForClick()
    {
        while (selectedPawn == this)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Verifica se o clique foi sobre uma casaa
                    if (hit.collider.CompareTag("Casa"))
                    {
                        Vector3 targetPos = hit.point;

                        // Ajusta a posi��o da pe�a para o centro da casa
                        targetPos = new Vector3(Mathf.Round(targetPos.x), transform.position.y, Mathf.Round(targetPos.z));

                        // Verifica se a posi��o  � uma casa adjacente e se est� vazia
                        if (IsAdjacent(targetPos) && IsPositionEmpty(targetPos))
                        {
                            targetPosition = targetPos;
                            isMoving = true;
                            DeselectPawn(); // Deseleciona a pe�a ap�s o movimento
                            yield break; // Move quando a posi��o da casa � definida
                        }
                    }
                }
            }
            yield return null; // Espera para verificar novamente
        }
    }

    // Verifica se a posi��o da casa� adjacente � posi��o atual
    private bool IsAdjacent(Vector3 targetPos)
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(targetPos.x, 0, targetPos.z));
        return distance == 1.0f; // Verifica se a dist�ncia � exatamente 1 unidade (uma casa)
    }

    // Verifica se a posi��o da casa est� vazia
    private bool IsPositionEmpty(Vector3 targetPos)
    {
        Collider[] hitColliders = Physics.OverlapBox(targetPos, new Vector3(0.5f, 0.1f, 0.5f));
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("ChessPawn")) // Verifica se h� uma pe�a na posi��o
            {
                return false; // A posi��o est� ocupada
            }
        }
        return true; // A posi��o est� vazia
    }
}
