using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 

public class WhitePawnManager : MonoBehaviour
{
    private static WhitePawnManager selectedPawn = null; // Peça atualmente selecionada

    private bool mouseOver = false; //aciona ao passar o mouse em cima do peão
    public Color hoverColor; //definindo cor ao passar mouse em cima do peão
    private Renderer rend; //variável para renderer
    private Color startColor; //definindo cor inicial da peça

    public float moveSpeed = 2f; // Velocidade de movimentação
    private Vector3 targetPosition; // Posição alvo para movimentação
    private Vector3 posicaoOriginal;
    private bool isMoving = false; // Flag para verificar se a peça está se movendo
    public TabuleiroDamas tabuleiro; //variável para usar ao chamar objetos da classe tabuleiro
    public whiteArcherManager arqueiroBranco; //variável para usar ao chamar objetos da classe tabuleiro
    private List<GameObject> casasDisponiveis = new List<GameObject>(); // Casas disponíveis para movimento
    private Dictionary<GameObject, Color> casaCoresOriginais = new Dictionary<GameObject, Color>(); // Cores originais das casas
    public GameObject whiteArcher, peao, whiteMage;


    public Vector2Int posAtual, novaPos;

    private void Start()
    {
        peao = this.gameObject;
        tabuleiro = FindObjectOfType<TabuleiroDamas>();
        arqueiroBranco = FindObjectOfType<whiteArcherManager>();

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        // Armazena as cores originais das casas
        foreach (var casa in GameObject.FindGameObjectsWithTag("Casa"))
        {
            casaCoresOriginais[casa] = casa.GetComponent<Renderer>().material.color;
        }
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (selectedPawn != this) // Não altera a cor se a peça estiver selecionada
        {
            rend.material.color = startColor;
        }
        mouseOver = false;
    }

    private void OnMouseDown()
    {
        if (mouseOver)
        {
            if (selectedPawn == this)
            {
                DeselectPawn();
            }
            else
            {
                posicaoOriginal = transform.position;
                SelectPawn();
            }
        }
    }

    public void promoteArcher()
    {
        //promover peão para outra classe
        Vector3 promotePosition = this.transform.position;
        promotePosition.y += 0.8f;
        Instantiate(whiteArcher, promotePosition, whiteArcher.transform.rotation);
        DeselectPawn();
        Destroy(peao);
    }

    public void promoteMage()
    {
        //promover peão para outra classe
        Vector3 promotePosition = this.transform.position;
        promotePosition.y += 1.1f;
        Instantiate(whiteMage, promotePosition, whiteMage.transform.rotation);
        DeselectPawn();
        Destroy(peao);
    }

    private void Update()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition;
                tabuleiro.ocupaCasa((int)targetPosition.x, (int)targetPosition.z);
                isMoving = false; // Interrompe o movimento quando chega ao alvo
            }
        }
    }

    private void SelectPawn()
    {
        if (isMoving || selectedPawn != null)
            return;

        selectedPawn = this;
        rend.material.color = hoverColor; // Mantém a cor de seleção
        tentativa();
        //MostrarCasasDisponiveis(); // Mostra a casas disponíveis para movimento
        StartCoroutine(WaitForClick());
    }

    
    public void DeselectPawn()
    {
        selectedPawn = null;
        rend.material.color = startColor; // Restaura a cor original
        RestaurarCasasDisponiveis(); // Restaura a cor das casas
    }

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
                    if (hit.collider.CompareTag("Casa"))
                    {
                        Vector3 targetPos = hit.point;

                        // Ajusta a posição alvo para o centro da casa
                        targetPos = new Vector3((int)(targetPos.x), transform.position.y, transform.position.z);
                        //targetPos = new Vector3((int)(targetPos.x), transform.position.y, (int)(targetPos.z));

                        // Verifica se a posição alvo é uma casa adjacente
                        //if (IsAdjacent(targetPos))
                        // Verifica se a posição é uma casa adjacente e se está vazia
                        //if (IsAdjacent(targetPos) && IsPositionEmpty(targetPos))
                        if (IsPositionEmpty(targetPos))
                        {
                            targetPosition = targetPos;

                            //verificar se a casa é na frente do peão
                            foreach (GameObject c in casasDisponiveis)
                                {
                                    if (targetPos.x == c.transform.position.x && targetPos.y -0.3f == c.transform.position.y)
                                    {
                                        tabuleiro.desocupaCasa((int)posicaoOriginal.x, (int)posicaoOriginal.z);
                                        isMoving = true;
                                        DeselectPawn(); // Deseleciona a peça após o movimento
                                        yield break; // Move quando a posição alvo é definida
                                    }                                        
                                }

                            //isMoving = true;
                            //DeselectPawn(); // Deseleciona a peça após o movimento
                            //yield break; // Move quando a posição alvo é definida
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(2))
            {
                promoteArcher();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                promoteMage();
            }
            yield return null; // Espera para verificar novamente
        }
    }

    /*private bool IsAdjacent(Vector3 targetPos)
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(targetPos.x, 0, targetPos.z));
        return distance == 1.0f; // Verifica se a distância é exatamente uma casa
    }*/

    private bool IsPositionEmpty(Vector3 targetPos)
    {
        Collider[] hitColliders = Physics.OverlapBox(targetPos, new Vector3(0.5f, 0.1f, 0.5f));
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("ChessPawn")) // Verifica se há uma peça na posição
            {
                return false; // A posição está ocupada
            }
        }
        return true; // A posição está vazia
    }


    public void tentativa()
    {
        casasDisponiveis.Clear();
        Vector3 currentPos = transform.position;
        posAtual = new Vector2Int((int)currentPos.x, (int)currentPos.z);

        if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z) == false) //checa a casa da frente
        {
            AdicionarCasaSeVazia((int)currentPos.x + 1, (int)currentPos.z);

            if ((tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z) == false) && transform.position.x == 0)
            {
                AdicionarCasaSeVazia((int)currentPos.x + 2, (int)currentPos.z);
            }
        }
    }

    private void AdicionarCasaSeVazia(int x, int y)
    {
        if (x >= 0 && x < tabuleiro._casaOcupada.GetLength(0) && y >= 0 && y < tabuleiro._casaOcupada.GetLength(1))
        {
            if (tabuleiro.IsPositionEmpty(new Vector3(x, 0, y)))
            {
                GameObject casa = GameObject.Find($"Casa {x},{y}");
                if (casa != null)
                {
                    casa.GetComponent<Renderer>().material.color = Color.yellow; // Muda a cor da casa
                    casasDisponiveis.Add(casa);
                }
            }
        }
    }

    private void MostrarCasasDisponiveis()
    {
        casasDisponiveis.Clear(); // Limpa a lista anterior

        Vector3 currentPos = transform.position;
        // Vector2Int posAtual = new Vector2Int(Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.z));
        posAtual = new Vector2Int(Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.z));

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dz = -1; dz <= 1; dz++)
            {
                if (Mathf.Abs(dx) == Mathf.Abs(dz)) continue; // Ignora diagonais

                //Vector2Int novaPos = new Vector2Int(posAtual.x + dx, posAtual.y + dz);
                //Vector2Int novaPos = new Vector2Int(posAtual.x + 1, posAtual.y);
                novaPos = new Vector2Int(posAtual.x + 1, posAtual.y);
                if (novaPos.x >= 0 && novaPos.x < tabuleiro._casaOcupada.GetLength(0) && novaPos.y >= 0 && novaPos.y < tabuleiro._casaOcupada.GetLength(1))
                {
                    // Verifica se a posição está vazia
                    if (tabuleiro.IsPositionEmpty(new Vector3(novaPos.x, 0, novaPos.y)))
                    {
                        // Destaca a casa
                        GameObject casa = GameObject.Find($"Casa {novaPos.x},{novaPos.y}");
                        if (casa != null)
                        {
                            casa.GetComponent<Renderer>().material.color = Color.yellow; // Muda a cor da casa
                            casasDisponiveis.Add(casa);
                        }
                    }
                }
            }
        }
    }

    private void RestaurarCasasDisponiveis()
    {
        foreach (GameObject casa in casasDisponiveis)
        {
            if (casaCoresOriginais.TryGetValue(casa, out Color originalColor))
            {
                casa.GetComponent<Renderer>().material.color = originalColor; // Restaura a cor original
            }
        }
    }
}
