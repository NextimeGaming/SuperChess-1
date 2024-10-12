using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteArcherManager : MonoBehaviour
{
    private static whiteArcherManager selectedArcher = null;

    private bool mouseOver = false;
    public Color hoverColor;
    private Renderer rend;
    private Color startColor;

    public TabuleiroDamas tabuleiro;
    private List<GameObject> casasDisponiveis = new List<GameObject>();
    private Dictionary<GameObject, Color> casaCoresOriginais = new Dictionary<GameObject, Color>();

    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private Vector2Int posAtual;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        tabuleiro = FindObjectOfType<TabuleiroDamas>();

        foreach (var casa in GameObject.FindGameObjectsWithTag("Casa"))
        {
            casaCoresOriginais[casa] = casa.GetComponent<Renderer>().material.color;
        }
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
                isMoving = false;
                DeselectArcher();
            }
        }
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (selectedArcher != this)
        {
            rend.material.color = startColor;
        }
        mouseOver = false;
    }

    private void OnMouseDown()
    {
        if (mouseOver)
        {
            if (selectedArcher == this)
            {
                DeselectArcher();
            }
            else
            {
                SelectArcher();
            }
        }
    }

    private void SelectArcher()
    {
        if (isMoving || selectedArcher != null)
            return;

        selectedArcher = this;
        rend.material.color = hoverColor;
        MostrarCasasDisponiveisArqueiro();

        StartCoroutine(WaitForClick());
    }

    private void DeselectArcher()
    {
        selectedArcher = null;
        rend.material.color = startColor;
        RestaurarCasasDisponiveis();
        casasDisponiveis.Clear();
    }

    private IEnumerator WaitForClick()
    {
        while (selectedArcher == this)
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
                        targetPos = new Vector3(Mathf.Round(targetPos.x), 1.1f, Mathf.Round(targetPos.z));

                        if (IsPositionValid(targetPos))
                        {
                            targetPosition = targetPos;
                            isMoving = true;
                            DeselectArcher();
                            yield break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    private bool IsPositionValid(Vector3 targetPos)
    {
        return tabuleiro.IsPositionEmpty(targetPos) && IsAdjacent(targetPos) && IsDirectionFree(posAtual, targetPos);
    }

    private bool IsAdjacent(Vector3 targetPos)
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(targetPos.x, 0, targetPos.z));
        return distance <= 2.0f; // Permite movimento nas diagonais
    }

    private bool IsDirectionFree(Vector2Int currentPos, Vector3 targetPos)
    {
        int deltaX = Mathf.RoundToInt(targetPos.x) - currentPos.x;
        int deltaY = Mathf.RoundToInt(targetPos.z) - currentPos.y;

        // Verifica se está movendo na horizontal
        if (deltaY == 0)
        {
            
            if (deltaX > 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x + 1, 0, currentPos.y));
            }
            
            else if (deltaX < 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x - 1, 0, currentPos.y));
            }
        }
        // Verifica se está movendo na vertical
        else if (deltaX == 0)
        {
            
            if (deltaY > 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x, 0, currentPos.y + 1));
            }
            
            else if (deltaY < 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x, 0, currentPos.y - 1));
            }
        }

        // Para movimentos diagonais ou inválidos
        return true;
    }

    private bool IsAdjacentHousesFree(Vector2Int currentPos)
    {
        // Verifica se as casas adjacentes estão vazias
        int[][] directions = new int[][]
        {
            new int[] { 1, 0 }, // Direita
            new int[] { -1, 0 }, // Esquerda
            new int[] { 0, 1 }, // Cima
            new int[] { 0, -1 }, // Baixo
            new int[] { 1, 1 }, // Diagonal direita-cima
            new int[] { 1, -1 }, // Diagonal direita-baixo
            new int[] { -1, 1 }, // Diagonal esquerda-cima
            new int[] { -1, -1 } // Diagonal esquerda-baixo
        };

        foreach (var dir in directions)
        {
            int x = currentPos.x + dir[0];
            int y = currentPos.y + dir[1];

            if (x >= 0 && x < tabuleiro._casaOcupada.GetLength(0) && y >= 0 && y < tabuleiro._casaOcupada.GetLength(1))
            {
                if (!tabuleiro.IsPositionEmpty(new Vector3(x, 0, y)))
                {
                    return false; // Casa ocupada
                }
            }
        }

        return true; // as casas estão livres
    }

    private void MostrarCasasDisponiveisArqueiro()
    {
        casasDisponiveis.Clear();
        Vector3 currentPos = transform.position;
        posAtual = new Vector2Int(Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.z));

        // Adiciona casas diagonais
        AdicionarCasaSeVazia(posAtual.x + 1, posAtual.y + 1);
        AdicionarCasaSeVazia(posAtual.x + 1, posAtual.y - 1);
        AdicionarCasaSeVazia(posAtual.x - 1, posAtual.y + 1);
        AdicionarCasaSeVazia(posAtual.x - 1, posAtual.y - 1);

        // Adiciona casas em linha reta 
        AdicionarCasaSeVaziaSeVazia(posAtual.x + 2, posAtual.y);
        AdicionarCasaSeVaziaSeVazia(posAtual.x - 2, posAtual.y);
        AdicionarCasaSeVaziaSeVazia(posAtual.x, posAtual.y + 2);
        AdicionarCasaSeVaziaSeVazia(posAtual.x, posAtual.y - 2);

        // Adiciona casas adjacentes 
        AdicionarCasaSeVaziaSeVazia(posAtual.x + 1, posAtual.y);
        AdicionarCasaSeVaziaSeVazia(posAtual.x - 1, posAtual.y);
        AdicionarCasaSeVaziaSeVazia(posAtual.x, posAtual.y + 1);
        AdicionarCasaSeVaziaSeVazia(posAtual.x, posAtual.y - 1);
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

    private void AdicionarCasaSeVaziaSeVazia(int x, int y)
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
